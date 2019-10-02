using System;
using System.Collections.Generic;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.AppRequests {

    public class AppTaskResult : BaseTaskResult {
        
        public App App { get; set; }

        public AppTaskResult() : base() {

            var createdDate = DateTime.UtcNow;

            App = new App() {

                Id = 0,
                Name = string.Empty,
                License = string.Empty,
                OwnerId = 0,
                DateCreated = createdDate,
                DateUpdated = createdDate,
                DevUrl = string.Empty,
                LiveUrl = string.Empty,
                Users = new List<UserApp>(),
                IsActive = false
            };
        }
    }
}
