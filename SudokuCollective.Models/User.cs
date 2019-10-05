using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SudokuCollective.Models.Interfaces;

namespace SudokuCollective.Models {

    public class User : IUser, IDBEntry {

        private string _userName;

        public int Id { get; set; }
        public string UserName {

            get {

                return _userName;
            }

            set {

                if (!string.IsNullOrEmpty(value)) {

                    var regex = new Regex("^[a-zA-Z0-9-._]*$");

                    if (regex.IsMatch(value)) {

                        _userName = value;
                    }

                } else {

                    _userName = string.Empty;
                }
            } 
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FullName { 
            get => string.Format("{0} {1}", FirstName, LastName); 
        }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public ICollection<Game> Games { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<UserApp> Apps { get; set; }

        public User(
            string firstName, 
            string lastName, 
            string password) : this() {

            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }

        public User() {

            Id = 0;
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateTime.UtcNow;
            Games = new List<Game>();
            Roles = new List<UserRole>();
            Apps = new List<UserApp>();
            
            if (string.IsNullOrEmpty(FirstName)) {

                FirstName = string.Empty;
            }
            
            if (string.IsNullOrEmpty(LastName)) {

                LastName = string.Empty;
            }
            
            if (string.IsNullOrEmpty(Password)) {

                Password = string.Empty;
            }

            IsActive = true;
        }

        [JsonConstructor]
        public User(
            int id, 
            string userName,
            string firstName, 
            string lastName, 
            string nickName,
            DateTime dateCreated, 
            DateTime dateUpdated, 
            string email, 
            string password,
            bool isActive) : this() {

            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            NickName = nickName;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
            Email = email;
            Password = password;
            IsActive = isActive;
        }

        public void ActivateUser() {

            IsActive = true;
        }

        public void DeactiveUser() {

            IsActive = false;
        }
    }
}
