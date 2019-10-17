using System;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.WebApi.Models.ResultModels.UserRequests {

    public class UserResult : BaseResult {
        
        public User User { get; set; }

        public UserResult() : base() {

            var createdDate = DateTime.UtcNow;

            User = new User() {

                Id = 0,
                UserName = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty,
                NickName = string.Empty,
                DateCreated = createdDate,
                DateUpdated = createdDate,
                Email = string.Empty,
                Password = string.Empty
            };
        }
    }
}
