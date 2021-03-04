using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Models
{
    public class App : IApp
    {
        #region Fields
        private List<UserApp> _users = new List<UserApp>();
        private TimeFrame _timeFrame;
        private int _accessDuration;
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
        public bool InDevelopment { get; set; }
        public bool PermitSuperUserAccess { get; set; }
        public bool PermitCollectiveLogins { get; set; }
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
        public bool DisableCustomUrls { get; set; }
        public string CustomEmailConfirmationDevUrl { get; set; }
        public string CustomEmailConfirmationLiveUrl { get; set; }
        public string CustomPasswordResetDevUrl { get; set; }
        public string CustomPasswordResetLiveUrl { get; set; }
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
        public TimeFrame TimeFrame 
        { 
            get
            {
                return _timeFrame;
            }

            set
            {
                _timeFrame = value;

                if (value == TimeFrame.HOURS && AccessDuration < 23)
                {
                    AccessDuration = 23;
                }
                else if (value == TimeFrame.DAYS && AccessDuration < 31)
                {
                    AccessDuration = 31;
                }
                else if (value == TimeFrame.MONTHS && AccessDuration < 12)
                {
                    AccessDuration = 12;
                }
                else
                {

                }
            }
        }
        public int AccessDuration 
        {
            get
            {
                return _accessDuration;
            }
            
            set
            {
                if (TimeFrame == TimeFrame.SECONDS)
                {
                    if (0 < value || value <= 59)
                    {
                        _accessDuration = value;
                    }
                }
                else if (TimeFrame == TimeFrame.MINUTES)
                {
                    if (0 < value || value <= 59)
                    {
                        _accessDuration = value;
                    }
                }
                else if (TimeFrame == TimeFrame.HOURS)
                {
                    if (0 < value || value <= 23)
                    {
                        _accessDuration = value;
                    }
                }
                else if (TimeFrame == TimeFrame.DAYS)
                {
                    if (0 < value || value <= 31)
                    {
                        _accessDuration = value;
                    }
                }
                else
                {
                    if (0 < value || value <= 12)
                    {
                        _accessDuration = value;
                    }
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
            PermitSuperUserAccess = false;
            PermitCollectiveLogins = true;
            InDevelopment = true;
            DisableCustomUrls = true;
            CustomEmailConfirmationDevUrl = string.Empty;
            CustomEmailConfirmationLiveUrl = string.Empty;
            CustomPasswordResetDevUrl = string.Empty;
            CustomPasswordResetLiveUrl = string.Empty;
            Users = new List<UserApp>();
            TimeFrame = TimeFrame.DAYS;
            AccessDuration = 1;
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
            bool permitSuperUserAccess,
            bool permitCollectiveLogins,
            bool inDevelopment,
            bool disableCustomUrls,
            string customEmailConfirmationDevUrl,
            string customEmailConfirmationLiveUrl,
            string customPasswordResetDevUrl,
            string customPasswordResetLiveUrl,
            TimeFrame timeFrame,
            int accessDuration,
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
            PermitSuperUserAccess = permitSuperUserAccess;
            PermitCollectiveLogins = permitCollectiveLogins;
            InDevelopment = inDevelopment;
            DisableCustomUrls = disableCustomUrls;
            CustomEmailConfirmationDevUrl = customEmailConfirmationDevUrl;
            CustomEmailConfirmationLiveUrl = customEmailConfirmationLiveUrl;
            CustomPasswordResetDevUrl = customPasswordResetDevUrl;
            CustomPasswordResetLiveUrl = customPasswordResetLiveUrl;
            TimeFrame = timeFrame;
            AccessDuration = accessDuration;
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
