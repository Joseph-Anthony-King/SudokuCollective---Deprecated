using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuCollective.WebApi.Models {

    public class PageListModel {

        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public SortValue SortBy { get; set; }
        public bool OrderByDescending { get; set; }

        public PageListModel () {

            Page = 0;
            ItemsPerPage = 0;
            SortBy = SortValue.NULL;
            OrderByDescending = false;
        }

        public PageListModel (
            int page, 
            int itemsPerPage, 
            int sortValue, 
            bool orderByDescending) {

            Page = page;
            ItemsPerPage = itemsPerPage;
            SortBy = (SortValue)sortValue;
            OrderByDescending = orderByDescending;
        }
    }

    public enum SortValue {

        NULL,
        ID,
        FIRSTNAME,
        LASTNAME,
        FULLNAME,
        NICKNAME,
        NAME,
        DATECREATED
    }
}
