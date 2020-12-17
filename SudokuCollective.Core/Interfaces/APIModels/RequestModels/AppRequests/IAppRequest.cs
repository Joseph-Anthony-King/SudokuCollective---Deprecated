﻿namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IAppRequest : IBaseRequest
    {
        string Name { get; set; }
        string DevUrl { get; set; }
        string LiveUrl { get; set; }
        bool InProduction { get; set; }
    }
}