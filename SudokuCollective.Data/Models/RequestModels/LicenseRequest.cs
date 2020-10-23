using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class LicenseRequest : ILicenseRequest
    {
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }

        public LicenseRequest()
        {
            Name = string.Empty;
            OwnerId = 0;
            DevUrl = string.Empty;
            LiveUrl = string.Empty;
        }
    }
}
