using System.ComponentModel.DataAnnotations;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Validation.Attributes;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class AppRequest : IAppRequest
    {
        [Required, GuidRegex(ErrorMessage = "License must be a valid Guid in the form of abcdefgh-1234-abcd-1234-abcdefghijkl")]
        public string License { get; set; }
        [Required]
        public int RequestorId { get; set; }
        [Required]
        public int AppId { get; set; }
        [PaginatorValidated]
        public IPaginator Paginator { get; set; }
        public string Name { get; set; }
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }
        public bool IsActive { get; set; }
        public bool InDevelopment { get; set; }
        public bool PermitSuperUserAccess { get; set; }
        public bool PermitCollectiveLogins { get; set; }
        public bool DisableCustomUrls { get; set; }
        public string CustomEmailConfirmationAction { get; set; }
        public string CustomPasswordResetAction { get; set; }
        public TimeFrame TimeFrame { get; set; }
        public int AccessDuration { get; set; }

        public AppRequest() : base()
        {
            Name = string.Empty;
            DevUrl = string.Empty;
            LiveUrl = string.Empty;
            IsActive = false;
            InDevelopment = false;
            PermitSuperUserAccess = false;
            PermitCollectiveLogins = false;
            DisableCustomUrls = false;
            CustomEmailConfirmationAction = string.Empty;
            CustomPasswordResetAction = string.Empty;

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
