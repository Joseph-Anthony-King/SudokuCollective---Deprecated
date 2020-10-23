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
        List<IGame> Games { get; set; }
        List<IUserRole> Roles { get; set; }
        List<IUserApp> Apps { get; set; }
        void ActivateUser();
        void DeactiveUser();
        void UpdateRoles();
    }
}
