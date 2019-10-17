using SudokuCollective.WebApi.Models.Pagination;

namespace SudokuCollective.WebApi.Models.RequestModels {

    public class BaseRequest {

        public string License { get; set; }
        public int RequestorId { get; set; }
        public PageListModel PageListModel { get; set; }

        public BaseRequest() {

            License = string.Empty;
            RequestorId = 0;
            PageListModel = new PageListModel();
        }
    }
}
