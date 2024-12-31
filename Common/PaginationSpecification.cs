namespace Stock_CMS.Common
{
    public abstract class PaginationSpecification
    {
        public int MaxResults { get; }

        public bool TotalNeeded { get; }

        public int NumberToSkip
        {
            get
            {
                var numberOfResultsPerPage = ReqNumberOfResultsPerPage <= MaxResults ? ReqNumberOfResultsPerPage : MaxResults;
                var pageNumber = ReqPageNumber <= 0 ? 1 : ReqPageNumber;
                return numberOfResultsPerPage * (pageNumber - 1);   //first number is 1
            }
        }

        public int NumberToTake => ReqNumberOfResultsPerPage <= MaxResults ? ReqNumberOfResultsPerPage : MaxResults;

        private int ReqNumberOfResultsPerPage { get; }

        private int ReqPageNumber { get; }

        protected PaginationSpecification(int maxResults, int requestedNumberOfResults, int requestedPageNumber, bool totalNeeded = true)
        {
            ReqNumberOfResultsPerPage = requestedNumberOfResults;
            ReqPageNumber = requestedPageNumber;
            MaxResults = maxResults;
            TotalNeeded = totalNeeded;
        }

        public int TotalRecords { get; set; }
    }
}
