using SudokuCollective.Core.Models;
using System;
using System.Collections.Generic;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IUser : IEntityBase
    {
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string NickName { get; set; }
        string FullName { get; }
        string Email { get; set; }
        string Password { get; set; }
        bool IsActive { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        List<Game> Games { get; set; }
        List<UserRole> Roles { get; set; }
        List<UserApp> Apps { get; set; }
        void ActivateUser();
        void DeactiveUser();
        void UpdateRoles();
    }
}
