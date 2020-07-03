using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Domain.Models {

    public class User : IUser {

        private string _userName;
        private bool _isSuperUser;
        private bool _isAdmin;

        public int Id { get; set; }
        [Required]
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
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FullName { 
            get => string.Format("{0} {1}", FirstName, LastName); 
        }
        [Required]
        public string Email { get; set; }
        [JsonIgnore]
        [Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuperUser {

            get {

                _isSuperUser = false;

                if (Roles != null) {

                    foreach (var role in Roles) {

                        if (role.Role.RoleLevel == RoleLevel.SUPERUSER) {

                            _isSuperUser = true;
                        }
                    }
                }

                return _isSuperUser;
            }

            set {

                _isSuperUser = value;
            }
        }
        public bool IsAdmin {

            get {

                _isAdmin = false;

                if (Roles != null) {

                    foreach (var role in Roles) {

                        if (role.Role.RoleLevel == RoleLevel.ADMIN) {

                            _isAdmin = true;
                        }
                    }
                }

                return _isAdmin;
            }

            set {

                _isAdmin = value;
            }
        }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public ICollection<Game> Games { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<UserApp> Apps { get; set; }

        public User(
            string firstName, 
            string lastName, 
            string password) : this() {

            var dateUserCreated = DateTime.UtcNow;

            FirstName = firstName;
            LastName = lastName;
            Password = password;
            DateCreated = dateUserCreated;
            IsActive = true;
            IsSuperUser = false;
            IsAdmin = false;
        }

        public User() {

            Id = 0;
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            DateCreated = DateTime.MinValue;
            DateUpdated = DateTime.MinValue;
            IsActive = false;
            Games = new List<Game>();
            Roles = new List<UserRole>();
            Apps = new List<UserApp>();
        }

        [JsonConstructor]
        public User(
            int id, 
            string userName,
            string firstName, 
            string lastName, 
            string nickName,
            string email, 
            string password,
            bool isActive,
            DateTime dateCreated,
            DateTime dateUpdated) {

            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            NickName = nickName;
            Email = email;
            Password = password;
            IsActive = isActive;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
        }

        public void ActivateUser() {

            IsActive = true;
        }

        public void DeactiveUser() {

            IsActive = false;
        }

        public void UpdateRoles() {

            if (Roles != null) {

                foreach (var role in Roles) {

                    if (role.Role.RoleLevel == RoleLevel.SUPERUSER) {

                        IsSuperUser = true;
                    
                    } else if (role.Role.RoleLevel == RoleLevel.ADMIN) {
                        
                        _isAdmin = true;

                    } else {

                        // do nothing...
                    }
                }
            }
        }
    }
}
