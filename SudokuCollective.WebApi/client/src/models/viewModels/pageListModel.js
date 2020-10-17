import { SortValue } from "../viewModels/sortValue";

class PageListModel {

    constructor(page, itemsPerPage, sortBy, orderByDescending, includeCompletedGames) {

        if (!arguments.length) {

            this.page = 0;
            this.itemsPerPage = 0;
            this.sortBy = SortValue[0].value;
            this.orderByDescending = false;
            this.includeCompletedGames = false;

        } else {

            this.page = page;
            this.itemsPerPage = itemsPerPage;
            this.sortBy = sortBy;
            this.orderByDescending = orderByDescending;
            this.includeCompletedGames = includeCompletedGames;
        }
    }
}

export default PageListModel;