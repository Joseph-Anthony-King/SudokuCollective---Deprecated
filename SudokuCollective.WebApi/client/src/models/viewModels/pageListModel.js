class PageListModel {

    constructor(page, itemsPerPage, sortBy, orderByDescending, includeCompletedGames) {

        this.page = page;
        this.itemsPerPage = itemsPerPage;
        this.sortBy = sortBy;
        this.orderByDescending = orderByDescending;
        this.includeCompletedGames = includeCompletedGames;
    }
}

export default PageListModel;