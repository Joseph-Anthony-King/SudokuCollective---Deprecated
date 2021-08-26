using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class LicenseResult : ILicenseResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public string License { get; set; }

        public LicenseResult() : base()
        {
            IsSuccess = true;
            IsFromCache = true;
            Message = string.Empty;
            License = string.Empty;
        }
    }
}
