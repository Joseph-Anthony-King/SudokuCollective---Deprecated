using System;
using System.Collections.Generic;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IApp : IEntityBase
    {
        string Name { get; set; }
        string License { get; set; }
        int OwnerId { get; set; }
        string DevUrl { get; set; }
        string LiveUrl { get; set; }
        bool IsActive { get; set; }
        bool InDevelopment { get; set; }
        string CustomEmailConfirmationDevUrl { get; set; }
        string CustomEmailConfirmationLiveUrl { get; set; }
        bool UseCustomEmailConfirmationUrl { get; }
        string CustomPasswordUpdateDevUrl { get; set; }
        string CustomPasswordUpdateLiveUrl { get; set; }
        bool UseCustomPasswordUpdateUrl { get; }
        int GameCount { get; }
        int UserCount { get; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        List<UserApp> Users { get; set; }
        public string GetLicense(int id, int ownerId);
        public void ActivateApp();
        public void DeactivateApp();
    }
}
