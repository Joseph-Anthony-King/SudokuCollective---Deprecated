using System;
using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.DataModels
{
    public interface IRepositoryResponse
    {
        bool Success { get; set; }
        Exception Exception { get; set; }
        IEntityBase Object { get; set; }
        List<IEntityBase> Objects { get; set; }
    }
}
