using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Test.MockRepositories
{
    public class MockUsersRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IUsersRepository<User>> UsersRepositorySuccessfulRequest { get; set; }
        internal Mock<IUsersRepository<User>> UsersRepositoryFailedRequest { get; set; }
        internal Mock<IUsersRepository<User>> UsersRepositoryEmailFailedRequest { get; set; }
        internal Mock<IUsersRepository<User>> UsersRepositoryInitiatePasswordSuccessful { get; set; }
        internal Mock<IUsersRepository<User>> UsersRepositoryResendEmailConfirmationSuccessful { get; set; }

        public MockUsersRepository(DatabaseContext ctxt)
        {
            context = ctxt;
            var todaysDate = DateTime.UtcNow;

            UsersRepositorySuccessfulRequest = new Mock<IUsersRepository<User>>();
            UsersRepositoryFailedRequest = new Mock<IUsersRepository<User>>();
            UsersRepositoryEmailFailedRequest = new Mock<IUsersRepository<User>>();
            UsersRepositoryInitiatePasswordSuccessful = new Mock<IUsersRepository<User>>();
            UsersRepositoryResendEmailConfirmationSuccessful = new Mock<IUsersRepository<User>>();

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.Add(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = new User(
                            4,
                            "TestUser3",
                            "Test",
                            "User 3",
                            "Test User 3",
                            "TestUser3@example.com",
                            true,
                            false,
                            "password",
                            false,
                            true,
                            todaysDate,
                            DateTime.MinValue)
                        {
                            Apps = new List<UserApp>() 
                            { 
                                new UserApp() 
                                { 
                                    App = new App()
                                } 
                            }
                        }
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.GetByUserName(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context
                            .Users
                            .FirstOrDefault(u => u.UserName.Equals("TestSuperUser"))
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.GetByEmail(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context
                            .Users
                            .FirstOrDefault(u => u.Email.Equals("TestSuperUser@example.com"))
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context
                            .Users
                            .ToList()
                            .ConvertAll(u => (IEntityBase)u)
                    } as IRepositoryResponse)) ;

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.Update(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.UpdateRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Users.ToList().ConvertAll(u => (IEntityBase)u)
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.Delete(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.DeleteRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.AddRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.ConfirmEmail(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.ActivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.DeactivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.PromoteUserToAdmin(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.IsUserRegistered(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.IsUserNameUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.IsEmailUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.IsUpdatedUserNameUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositorySuccessfulRequest.Setup(usersRepo =>
                usersRepo.IsUpdatedEmailUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.Add(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.GetByUserName(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.GetByEmail(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.Update(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.UpdateRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.Delete(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.DeleteRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.AddRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.ConfirmEmail(It.IsAny<IEmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.ActivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.DeactivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.PromoteUserToAdmin(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.IsUserRegistered(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.IsUserNameUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.IsEmailUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.IsUpdatedUserNameUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryFailedRequest.Setup(usersRepo =>
                usersRepo.IsUpdatedEmailUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.Add(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new User(
                            4,
                            "TestUser3",
                            "Test",
                            "User 3",
                            "Test User 3",
                            "TestUser3@example.com",
                            false,
                            false,
                            "password",
                            false,
                            true,
                            todaysDate,
                            DateTime.MinValue)
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.GetByUserName(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context
                            .Users
                            .FirstOrDefault(u => u.UserName.Equals("TestSuperUser"))
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.GetByEmail(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context
                            .Users
                            .FirstOrDefault(u => u.Email.Equals("TestSuperUser@example.com"))
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context
                            .Users
                            .ToList()
                            .ConvertAll(u => (IEntityBase)u)
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.Update(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.UpdateRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Users.ToList().ConvertAll(u => (IEntityBase)u)
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.Delete(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.DeleteRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.AddRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.ConfirmEmail(It.IsAny<IEmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.ActivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.DeactivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.PromoteUserToAdmin(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.IsUserRegistered(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.IsUserNameUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.IsEmailUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.IsUpdatedUserNameUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryEmailFailedRequest.Setup(usersRepo =>
                usersRepo.IsUpdatedEmailUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.Add(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new User(
                            4,
                            "TestUser3",
                            "Test",
                            "User 3",
                            "Test User 3",
                            "TestUser3@example.com",
                            true,
                            false,
                            "password",
                            false,
                            true,
                            todaysDate,
                            DateTime.MinValue)
                        {
                            Apps = new List<UserApp>()
                            {
                                new UserApp()
                                {
                                    App = new App()
                                }
                            }
                        }
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 3)
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.GetByUserName(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context
                            .Users
                            .FirstOrDefault(u => u.UserName.Equals("TestSuperUser"))
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.GetByEmail(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context
                            .Users
                            .FirstOrDefault(u => u.Email.Equals("TestSuperUser@example.com"))
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context
                            .Users
                            .ToList()
                            .ConvertAll(u => (IEntityBase)u)
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.Update(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.UpdateRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Users.ToList().ConvertAll(u => (IEntityBase)u)
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.Delete(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.DeleteRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.AddRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.ConfirmEmail(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.ActivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.DeactivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.PromoteUserToAdmin(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.IsUserRegistered(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.IsUserNameUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.IsEmailUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.IsUpdatedUserNameUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryInitiatePasswordSuccessful.Setup(usersRepo =>
                usersRepo.IsUpdatedEmailUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.Add(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new User(
                            4,
                            "TestUser3",
                            "Test",
                            "User 3",
                            "Test User 3",
                            "TestUser3@example.com",
                            true,
                            false,
                            "password",
                            false,
                            true,
                            todaysDate,
                            DateTime.MinValue)
                        {
                            Apps = new List<UserApp>()
                            {
                                new UserApp()
                                {
                                    App = new App()
                                }
                            }
                        }
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 3)
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.GetByUserName(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context
                            .Users
                            .FirstOrDefault(u => u.UserName.Equals("TestSuperUser"))
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.GetByEmail(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context
                            .Users
                            .FirstOrDefault(u => u.Email.Equals("TestSuperUser@example.com"))
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context
                            .Users
                            .ToList()
                            .ConvertAll(u => (IEntityBase)u)
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.Update(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.UpdateRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Users.ToList().ConvertAll(u => (IEntityBase)u)
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.Delete(It.IsAny<User>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.DeleteRange(It.IsAny<List<User>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.AddRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.ConfirmEmail(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Users.FirstOrDefault(u => u.Id == 1)
                    } as IRepositoryResponse));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.ActivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.DeactivateUser(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.PromoteUserToAdmin(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.IsUserRegistered(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.IsUserNameUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.IsEmailUnique(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.IsUpdatedUserNameUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UsersRepositoryResendEmailConfirmationSuccessful.Setup(usersRepo =>
                usersRepo.IsUpdatedEmailUnique(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));
        }
    }
}
