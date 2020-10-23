using SudokuCollective.Core.Enums;

namespace SudokuCollective.Core.Interfaces.APIModels.PageModels
{
    public interface IPageListModel
    {
        int Page { get; set; }
        int ItemsPerPage { get; set; }
        SortValue SortBy { get; set; }
        bool OrderByDescending { get; set; }
        bool IncludeCompletedGames { get; set; }
        bool IsNull();
    }
}
