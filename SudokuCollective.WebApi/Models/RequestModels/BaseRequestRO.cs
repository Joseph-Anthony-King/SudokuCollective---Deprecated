namespace SudokuCollective.WebApi.Models.RequestModels {

    public class BaseRequestRO {

        public string License { get; set; }
        public int RequestorId { get; set; }
        public PageListModel PageListModel { get; set; }
    }
}
