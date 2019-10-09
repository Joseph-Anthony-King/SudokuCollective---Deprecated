using System;
using System.Collections.Generic;

namespace SudokuCollective.Domain.Interfaces {

    public interface IApp {

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
