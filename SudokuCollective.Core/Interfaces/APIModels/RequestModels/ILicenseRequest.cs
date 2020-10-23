namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface ILicenseRequest
    {
       string Name { get; set; }
       int OwnerId { get; set; }
       string DevUrl { get; set; }
       string LiveUrl { get; set; }
    }
}
