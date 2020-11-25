using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IUserResult : IBaseResult
    {
        IUser User { get; set; }
        bool ConfirmationEmailSuccessfullySent { get; set; }
    }
}
