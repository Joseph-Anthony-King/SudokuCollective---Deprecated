using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class BaseRequest : IBaseRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPaginator Paginator { get; set; }

        public BaseRequest()
        {
            License = string.Empty;
            RequestorId = 0;
            AppId = 0;
            Paginator = new Paginator();
        }
    }
}
