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
        public List<Game> Games { get; set; }
        public List<Permission> Permissions { get; set; }
        public string Password { get; set; }

        public User(string firstName, 
            string lastName, 
            string password,
            Permission permission) : this() {
                
            if (permission.PermissionLevel == PermissionLevel.USER) {

                this.Permissions.Add(permission);
            }

            this.FirstName = firstName;
            this.LastName = lastName;
            this.Password = password;
        }

        public User() {

            this.DateCreated = GetCurrentTime();
            this.Games = new List<Game>();
            this.Permissions = new List<Permission>();
            
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

        public bool IsAdmin() {

            var result = this.Permissions.Any(p => p.PermissionLevel == PermissionLevel.ADMIN);

            return result;
        }

        public bool UpgradeToAdmin(Permission permission) {

            var result = false;

            if (permission.PermissionLevel == PermissionLevel.ADMIN) {

                this.Permissions.Add(permission);
                result = true;
            }

            return result;
        }

        protected virtual DateTime GetCurrentTime() {

            return DateTime.Now;
        }
    }
}
