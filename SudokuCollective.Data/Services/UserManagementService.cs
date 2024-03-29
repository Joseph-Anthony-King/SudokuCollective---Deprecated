﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.DataModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Data.Resiliency;

namespace SudokuCollective.Data.Services
{
    public class UserManagementService : IUserManagementService
    {
        #region Fields
        private readonly IUsersRepository<User> _usersRepository;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Constructors
        public UserManagementService(
            IUsersRepository<User> usersRepository,
            IDistributedCache distributedCache)
        {
            _usersRepository = usersRepository;
            _distributedCache = distributedCache;
        }
        #endregion

        #region Methods
        public async Task<bool> IsValidUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            try
            {
                var userResponse = await _usersRepository.GetByUserName(username);

                if (userResponse.Success)
                {
                    if ((IUser)userResponse.Object != null
                        && BCrypt.Net.BCrypt.Verify(password, ((IUser)userResponse.Object).Password))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserAuthenticationErrorType> ConfirmAuthenticationIssue(string username, string password, string license)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            try
            {
                var cachFactoryResponse = await CacheFactory.GetByUserNameWithCacheAsync(
                    _usersRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetUserByUsernameCacheKey, username, license),
                    CachingStrategy.Medium,
                    username,
                    license);

                var userResponse = (RepositoryResponse)cachFactoryResponse.Item1;

                if (userResponse.Success)
                {
                    if (!BCrypt.Net.BCrypt.Verify(password, ((IUser)userResponse.Object).Password))
                    {
                        return UserAuthenticationErrorType.PASSWORDINVALID;
                    }
                    else
                    {
                        return UserAuthenticationErrorType.NULL;
                    }
                }
                else if (!userResponse.Success && userResponse.Object == null)
                {
                    return UserAuthenticationErrorType.USERNAMEINVALID;
                }
                else
                {
                    return UserAuthenticationErrorType.NULL;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IAuthenticationResult> ConfirmUserName(string email, string license)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            try
            {
                var result = new AuthenticationResult();

                var cachFactoryResponse = await CacheFactory.GetByEmailWithCacheAsync(
                    _usersRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetUserByUsernameCacheKey, email, license),
                    CachingStrategy.Medium,
                    email,
                    result);

                var userResponse = (RepositoryResponse)cachFactoryResponse.Item1;
                result = (AuthenticationResult)cachFactoryResponse.Item2;

                if (userResponse.Success)
                {
                    result.IsSuccess = true;
                    result.Message = UsersMessages.UserNameConfirmedMessage;
                    result.UserName = ((User)userResponse.Object).UserName;

                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = UsersMessages.NoUserIsUsingThisEmailMessage;

                    return result;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
