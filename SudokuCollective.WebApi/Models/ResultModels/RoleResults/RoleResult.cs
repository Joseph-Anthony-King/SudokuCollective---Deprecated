using System.Collections.Generic;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.WebApi.Models.ResultModels.RoleRequests {

    public class RoleResult : BaseResult {
        
        public Role Role { get; set; }

        public RoleResult() : base() {

            Role = new Role() {

                Id = 0,
                Name = string.Empty,
                RoleLevel = RoleLevel.NULL,
                Users = new List<UserRole>()
            };
        }
    }
}