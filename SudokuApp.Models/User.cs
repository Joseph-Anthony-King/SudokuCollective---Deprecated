using System;
using System.Linq;
using System.Collections.Generic;
using SudokuApp.Models.Enums;
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
        public string Password { get; set; }
        public ICollection<Game> Games { get; set; }
        public ICollection<UserPermission> Permissions { get; set; }

        public User(string firstName, 
            string lastName, 
            string password) : this() {

            this.FirstName = firstName;
            this.LastName = lastName;
            this.Password = password;
        }

        public User() {

            this.DateCreated = GetCurrentTime();
            this.Games = new List<Game>();
            
            if (string.IsNullOrEmpty(this.FirstName)) {

                this.FirstName = string.Empty;
            }
            
            if (string.IsNullOrEmpty(this.LastName)) {

                this.LastName = string.Empty;
            }
            
            if (string.IsNullOrEmpty(this.Password)) {

                this.Password = string.Empty;
            }
        }

        protected virtual DateTime GetCurrentTime() {

            return DateTime.Now;
        }
    }
}
