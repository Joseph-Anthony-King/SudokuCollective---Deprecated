using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IUsersResult : IBaseResult
    {
        List<IUser> Users { get; set; }
    }
}
