using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Models
{
    public class App : IApp
    {
        #region Fields
        private List<UserApp> _users = new List<UserApp>();
        #endregion

        #region Properties
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        [Required]
        public string License { get; set; }
        [Required]
        public int OwnerId { get; set; }
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }
        public bool IsActive { get; set; }
        public bool AllowSuperUserAccess { get; set; }
        public bool InDevelopment { get; set; }
        public bool DisableCustomUrls { get; set; }
        public string CustomEmailConfirmationDevUrl { get; set; }
        public string CustomEmailConfirmationLiveUrl { get; set; }
        [JsonIgnore]
        public bool UseCustomEmailConfirmationUrl 
        {
            get
            {
                if (InDevelopment && !DisableCustomUrls && !string.IsNullOrEmpty(CustomEmailConfirmationDevUrl))
                {
                    return true;
                }
                else if (!InDevelopment && !DisableCustomUrls && !string.IsNullOrEmpty(CustomEmailConfirmationLiveUrl))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string CustomPasswordResetDevUrl { get; set; }
        public string CustomPasswordResetLiveUrl { get; set; }
        [JsonIgnore]
        public bool UseCustomPasswordResetUrl
        {
            get
            {
                if (InDevelopment && !DisableCustomUrls && !string.IsNullOrEmpty(CustomPasswordResetDevUrl))
                {
                    return true;
                }
                else if (!InDevelopment && !DisableCustomUrls && !string.IsNullOrEmpty(CustomPasswordResetLiveUrl))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public int GameCount
        {
            get
            {
                var result = 0;

                if (Users != null)
                {
                    foreach (var user in Users)
                    {
                        result += user.User.Games.Count;
                    }
                }

                return result;
            }
        }
        public int UserCount
        {
            get
            {
                if (Users != null)
                {
                    return Users.Count;
                }
                else
                {
                    return 0;
                }
            }
        }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public virtual List<UserApp> Users
        {

            get
            {
                return _users;
            }

            set
            {
                _users = value;
            }
        }

        #endregion

        #region Constructors
        public App()
        {
            Id = 0;
            Name = string.Empty;
            License = string.Empty;
            OwnerId = 0;
            DateCreated = DateTime.UtcNow;
            DevUrl = string.Empty;
            LiveUrl = string.Empty;
            IsActive = false;
            AllowSuperUserAccess = false;
            InDevelopment = true;
            DisableCustomUrls = true;
            CustomEmailConfirmationDevUrl = string.Empty;
            CustomEmailConfirmationLiveUrl = string.Empty;
            CustomPasswordResetDevUrl = string.Empty;
            CustomPasswordResetLiveUrl = string.Empty;
            Users = new List<UserApp>();
        }

        public App(
            string name, 
            string license,
            int ownerId, 
            string devUrl, 
            string liveUrl) : this()
        {
            Name = name;
            License = license;
            OwnerId = ownerId;
            DateCreated = DateTime.UtcNow;
            DevUrl = devUrl;
            LiveUrl = liveUrl;
        }

        [JsonConstructor]
        public App(
            int id,
            string name,
            string license,
            int ownerId,
            string devUrl,
            string liveUrl,
            bool isActive,
            bool allowSuperUserAccess,
            bool inDevelopment,
            bool disableCustomUrls,
            string customEmailConfirmationDevUrl,
            string customEmailConfirmationLiveUrl,
            string customPasswordResetDevUrl,
            string customPasswordResetLiveUrl,
            DateTime dateCreated,
            DateTime dateUpdated)
        {
            Id = id;
            Name = name;
            License = license;
            OwnerId = ownerId;
            DevUrl = devUrl;
            LiveUrl = liveUrl;
            IsActive = isActive;
            AllowSuperUserAccess = allowSuperUserAccess;
            InDevelopment = inDevelopment;
            DisableCustomUrls = disableCustomUrls;
            CustomEmailConfirmationDevUrl = customEmailConfirmationDevUrl;
            CustomEmailConfirmationLiveUrl = customEmailConfirmationLiveUrl;
            CustomPasswordResetDevUrl = customPasswordResetDevUrl;
            CustomPasswordResetLiveUrl = customPasswordResetLiveUrl;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
        }
        #endregion

        #region Methods
        public string GetLicense(int id, int ownerId)
        {
            var result = string.Empty;

            if (Id == id && OwnerId == id)
            {
                result = License;
            }

            return result;
        }

        public void ActivateApp()
        {
            IsActive = true;
        }

        public void DeactivateApp()
        {
            IsActive = false;
        }
        #endregion
    }
}
