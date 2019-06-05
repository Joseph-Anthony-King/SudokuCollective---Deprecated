using System;
using System.Collections.Generic;

namespace SudokuApp.Models.Interfaces { 

    public interface IUser {

        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string NickName { get; set; }
        string FullName { get; }
        DateTime DateCreated { get; set; }
        string Email { get; set; }
        List<Game> Games { get; set; }
    }
}
