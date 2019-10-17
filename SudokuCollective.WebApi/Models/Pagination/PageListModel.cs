using SudokuCollective.WebApi.Models.Enums;

namespace SudokuCollective.WebApi.Models.Pagination {

    public class PageListModel {

        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public SortValue SortBy { get; set; }
        public bool OrderByDescending { get; set; }
        public bool IncludeCompletedGames { get; set; }

        public PageListModel () {

            Page = 0;
            ItemsPerPage = 0;
            SortBy = SortValue.NULL;
            OrderByDescending = false;
            IncludeCompletedGames = false;
        }

        public PageListModel (
            int page, 
            int itemsPerPage, 
            int sortValue, 
            bool orderByDescending,
            bool includeCompletedGames) {

            Page = page;
            ItemsPerPage = itemsPerPage;
            SortBy = (SortValue)sortValue;
            OrderByDescending = orderByDescending;
            IncludeCompletedGames = includeCompletedGames;
        }
    }
}
