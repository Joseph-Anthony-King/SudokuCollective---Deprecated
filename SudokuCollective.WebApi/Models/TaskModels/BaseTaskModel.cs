namespace SudokuCollective.WebApi.Models.TaskModels {

    public class BaseTaskResult {

        public bool Success { get; set; }
        public string Message { get; set; }

        public BaseTaskResult() {

            Success = false;
            Message = string.Empty;
        }
    }
}
