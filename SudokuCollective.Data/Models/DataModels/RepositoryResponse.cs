using System;
using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.DataModels
{
    public class RepositoryResponse : IRepositoryResponse
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
        public IEntityBase Object { get; set; }
        public List<IEntityBase> Objects { get; set; }
        public string Token { get; set; }

        public RepositoryResponse()
        {
            Success = false;
            Exception = null;
            Object = null;
            Objects = null;
            Token = string.Empty;
        }
    }
}
