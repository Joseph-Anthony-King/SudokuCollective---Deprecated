using System;
using SudokuCollective.WebApi.Models.DTOModels;

namespace SudokuCollective.WebApi.Models.ResultModels.UserResults {

    public class AuthenticatedUserResult : BaseResult {

        public AuthenticatedUser User { get; set; }
        public string Token { get; set; }

        public AuthenticatedUserResult() {

            var createdDate = DateTime.UtcNow;

            User = new AuthenticatedUser() {

                Id = 0,
                UserName = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty,
                NickName = string.Empty,
                FullName = string.Empty,
                Email = string.Empty,
                IsActive = false,
                IsSuperUser = false,
                IsAdmin = false,
                DateCreated = createdDate,
                DateUpdated = createdDate
            };
        }

        public AuthenticatedUserResult(
            AuthenticatedUser user, 
            string token) {

            User = user;
            Token = token;
        }
    }
}
