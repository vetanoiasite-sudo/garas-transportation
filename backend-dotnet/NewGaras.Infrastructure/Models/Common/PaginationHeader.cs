namespace NewGarasAPI.Models.Common
{
    public class PaginationHeader
    {
        // the number of the current page
        public int CurrentPage { get; set; }

        //the total number of pages
        public int TotalPages { get; set; }

        //the count of items per each page
        public int ItemsPerPage { get; set; }

        //total count of items in the query
        public int TotalItems { get; set; }
    }

}
