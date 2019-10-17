namespace SudokuCollective.WebApi.Models.ResultModels {

    public class BaseResult {

        public bool Success { get; set; }
        public string Message { get; set; }

        public BaseResult() {

            Success = false;
            Message = string.Empty;
        }
    }
}
