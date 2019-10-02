using System;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.UserRequests {

    public class UserTaskResult : BaseTaskResult {
        
        public User User { get; set; }

        public UserTaskResult() : base() {

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
