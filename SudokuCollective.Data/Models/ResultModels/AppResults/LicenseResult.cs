using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class LicenseResult : ILicenseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string License { get; set; }

        public LicenseResult() : base()
        {
            License = string.Empty;
        }
    }
}
