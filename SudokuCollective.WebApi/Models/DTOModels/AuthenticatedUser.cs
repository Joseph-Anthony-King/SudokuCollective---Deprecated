using System;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.WebApi.Models.DTOModels {

    public class AuthenticatedUser {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public void UpdateWithUserInfo(IUser user) {

            Id = user.Id;
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            NickName = user.NickName;
            FullName = user.FullName;
            Email = user.Email;
            IsActive = user.IsActive;
            DateCreated = user.DateCreated;
            DateUpdated = user.DateUpdated;
        }
    }
}
