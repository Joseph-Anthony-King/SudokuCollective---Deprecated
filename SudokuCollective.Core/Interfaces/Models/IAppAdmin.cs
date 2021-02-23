using System;
using System.Collections.Generic;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IAppAdmin : IEntityBase
    {
        int AppId { get; set; }
        int UserId { get; set; }
        bool IsActive { get; set; }
    }
}