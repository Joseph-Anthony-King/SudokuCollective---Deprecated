using SudokuCollective.Core.Interfaces.APIModels.PageModels;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IBaseRequest
    {
        string License { get; set; }
        int RequestorId { get; set; }
        int AppId { get; set; }
        IPageListModel PageListModel { get; set; }
    }
}
