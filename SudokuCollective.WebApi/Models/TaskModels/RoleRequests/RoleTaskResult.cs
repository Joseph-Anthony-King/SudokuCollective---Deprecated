using System.Collections.Generic;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Enums;

namespace SudokuCollective.WebApi.Models.TaskModels.RoleRequests {

    public class RoleTaskResult : BaseTaskResult {
        
        public Role Role { get; set; }

        public RoleTaskResult() : base() {

            Role = new Role() {

                Id = 0,
                Name = string.Empty,
                RoleLevel = RoleLevel.NULL,
                Users = new List<UserRole>()
            };
        }
    }
}