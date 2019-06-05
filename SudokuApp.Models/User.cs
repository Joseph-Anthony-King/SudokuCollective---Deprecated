using System;
using System.Collections.Generic;
using SudokuApp.Models.Interfaces;

namespace SudokuApp.Models {

    public class User : IUser {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FullName { 
            get => string.Format("{0} {1}", FirstName, LastName); 
        }
        public DateTime DateCreated { get; set; }
        public string Email { get; set; }
        public List<Game> Games { get; set; }

        public User(string firstName, string lastName) {

            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateCreated = DateTime.Now;
        }

        public User() {

            this.DateCreated = DateTime.Now;
        }
    }
}
