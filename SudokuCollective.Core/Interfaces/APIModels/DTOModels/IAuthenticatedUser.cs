﻿using System;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.DTOModels
{
    public interface IAuthenticatedUser : IEntityBase
    {
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string NickName { get; set; }
        string FullName { get; set; }
        string Email { get; set; }
        bool IsEmailConfirmed { get; set; }
        bool ReceivedRequestToUpdateEmail { get; set; }
        bool ReceivedRequestToUpdatePassword { get; set; }
        bool IsActive { get; set; }
        bool IsSuperUser { get; set; }
        bool IsAdmin { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        void UpdateWithUserInfo(IUser user);
    }
}
