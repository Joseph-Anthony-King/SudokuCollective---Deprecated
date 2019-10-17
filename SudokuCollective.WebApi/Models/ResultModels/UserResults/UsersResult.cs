﻿using System.Collections.Generic;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.WebApi.Models.ResultModels.UserRequests {

    public class UsersResult : BaseResult {
        
        public IEnumerable<User> Users { get; set; }

        public UsersResult() : base() {

            Users = new List<User>();
        }
    }
}