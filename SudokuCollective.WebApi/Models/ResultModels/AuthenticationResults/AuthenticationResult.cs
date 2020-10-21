using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuCollective.WebApi.Models.ResultModels.AuthenticationResults {

    public class AuthenticationResult : BaseResult {

        public string UserName { get; set; }

        public AuthenticationResult() : base() {

            UserName = string.Empty;
        }
    }
}
