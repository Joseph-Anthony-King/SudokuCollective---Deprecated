using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IRolesResult : IBaseResult
    {
        List<IRole> Roles { get; set; }
    }
}
