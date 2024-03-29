using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Models
{
    public class User : IUser
    {
        #region Fields
        private string _userName;
        private string _email;
        private bool _isSuperUser;
        private bool _isAdmin;
        private List<Game> _games = new List<Game>();
        private List<UserRole> _roles = new List<UserRole>();
        private List<UserApp> _apps = new List<UserApp>();
        #endregion

        #region Properties
        public int Id { get; set; }
        [Required]
        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var regex = new Regex("^[^-]{1}?[^\"\']*$");

                    if (regex.IsMatch(value))
                    {
                        _userName = value;
                    }
                }
                else
                {
                    _userName = string.Empty;
                }
            }
        }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FullName
        {
            get => string.Format("{0} {1}", FirstName, LastName);
        }
        [Required]
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
                IsEmailConfirmed = false;
            }
        }
        public bool IsEmailConfirmed { get; set; }
        public bool ReceivedRequestToUpdateEmail { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string Password { get; set; }
        public bool ReceivedRequestToUpdatePassword { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuperUser
        {
            get
            {
                _isSuperUser = false;

                if (Roles != null)
                {
                    foreach (var role in Roles)
                    {
                        if (role.Role.RoleLevel == RoleLevel.SUPERUSER)
                        {
                            _isSuperUser = true;
                        }
                    }
                }

                return _isSuperUser;
            }

            set
            {
                
                _isSuperUser = value;
            }
        }
        public bool IsAdmin
        {
            get
            {
                _isAdmin = false;

                if (Roles != null)
                {
                    foreach (var role in Roles)
                    {
                        if (role.Role.RoleLevel == RoleLevel.ADMIN)
                        {
                            _isAdmin = true;
                        }
                    }
                }

                return _isAdmin;
            }

            set
            {
                _isAdmin = value;
            }
        }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public virtual List<Game> Games
        {
            get
            {
                return _games;
            }
            set
            {
                _games = value;
            }
        }
        public virtual List<UserRole> Roles
        {
            get
            {
                return _roles;
            }
            set
            {
                _roles = value;
            }
        }
        public virtual List<UserApp> Apps
        {
            get
            {
                return _apps;
            }
            set
            {
                _apps = value;
            }
        }
        #endregion

        #region Constructors
        public User(
            string firstName,
            string lastName,
            string password) : this()
        {
            var dateUserCreated = DateTime.UtcNow;

            FirstName = firstName;
            LastName = lastName;
            Password = password;
            ReceivedRequestToUpdatePassword = false;
            DateCreated = dateUserCreated;
            IsActive = true;
            IsSuperUser = false;
            IsAdmin = false;
            IsEmailConfirmed = false;
        }

        public User()
        {
            Id = 0;
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            Email = string.Empty;
            IsEmailConfirmed = false;
            ReceivedRequestToUpdateEmail = false;
            Password = string.Empty;
            ReceivedRequestToUpdatePassword = false;
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
            bool emailConfirmed,
            bool receivedRequestToUpdateEmail,
            string password,
            bool receivedRequestToUpdatePassword,
            bool isActive,
            DateTime dateCreated,
            DateTime dateUpdated)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            NickName = nickName;
            Email = email;
            IsEmailConfirmed = emailConfirmed;
            ReceivedRequestToUpdateEmail = receivedRequestToUpdateEmail;
            Password = password;
            ReceivedRequestToUpdatePassword = receivedRequestToUpdatePassword;
            IsActive = isActive;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
        }
        #endregion

        #region Methods
        public void ActivateUser()
        {
            IsActive = true;
        }

        public void DeactiveUser()
        {
            IsActive = false;
        }

        public void UpdateRoles()
        {
            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    if (role.Role.RoleLevel == RoleLevel.SUPERUSER)
                    {
                        IsSuperUser = true;
                    }
                    else if (role.Role.RoleLevel == RoleLevel.ADMIN)
                    {
                        _isAdmin = true;
                    }
                    else
                    {
                        // do nothing...
                    }
                }
            }
        }
        #endregion
    }
}
