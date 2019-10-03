using System;
using System.Collections.Generic;

namespace SudokuCollective.Models.Interfaces { 

    public interface IUser {

        string FirstName { get; set; }
        string LastName { get; set; }
        string NickName { get; set; }
        string FullName { get; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        ICollection<Game> Games { get; set; }
        ICollection<UserRole> Roles { get; set; }
        ICollection<UserApp> Apps { get; set; }
        bool IsActive { get; set; }
    }
}
