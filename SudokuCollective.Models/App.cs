using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SudokuCollective.Models.Interfaces;

namespace SudokuCollective.Models {

    public class App : IApp, IDBEntry {

        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string License { get; set; }
        public int OwnerId { get; set; }
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public ICollection<UserApp> Users { get; set; }

        public App() {

            Id = 0;
            Name = string.Empty;
            License = string.Empty;
            OwnerId = 0;
            DateCreated = DateTime.UtcNow;
            DevUrl = string.Empty;
            LiveUrl = string.Empty;
            IsActive = true;
        }

        public App(string name, string license, 
            int ownerId, string devUrl, string liveUrl) {

            Name = name;
            License = license;
            OwnerId = ownerId;
            DateCreated = DateTime.UtcNow;
            DevUrl = devUrl;
            LiveUrl = liveUrl;
            IsActive = true;
        }

        [JsonConstructor]
        public App(
            int id, 
            string name, 
            string license, 
            int ownerId, 
            DateTime dateCreated, 
            string devUrl, 
            string liveUrl,
            bool isActive) : this() {

            Id = id;
            Name = name;
            License = license;
            OwnerId = ownerId;
            DateCreated = dateCreated;
            DevUrl = devUrl;
            LiveUrl = liveUrl;
            IsActive = isActive;
        }
        
        public string GetLicense(int id, int ownerId) {

            var result = string.Empty;

            if (Id == id && OwnerId == id) {

                result = License;
            }

            return License;
        }

        public void ActivateApp() {
            
            IsActive = true;
        }

        public void DeactivateApp() {

            IsActive = false;
        }
    }
}
