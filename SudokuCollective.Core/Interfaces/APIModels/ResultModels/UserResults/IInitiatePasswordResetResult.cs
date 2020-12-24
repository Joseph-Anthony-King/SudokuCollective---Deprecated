using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults
{
    public interface IInitiatePasswordResetResult : IUserResult
    {
        IApp App { get; set; }
    }
}
