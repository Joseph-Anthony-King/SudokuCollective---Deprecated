using System;
using System.Collections.Generic;

namespace SudokuCollective.Models.Interfaces {

    public interface IApp {

        int Id { get; set; }
        string Name { get; set; }
        string License { get; set; }
        int OwnerId { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        string DevUrl { get; set; }
        string LiveUrl { get; set; }
        ICollection<UserApp> Users { get; set; }
        bool IsActive { get; set; }
    }
}
