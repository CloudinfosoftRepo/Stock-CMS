using NetTopologySuite.Index.HPRtree;

namespace Stock_CMS.Models
{

    public class PageResponse<T>
    {
        public int PageNumber { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
    public class FilterRule
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
    }
    public class PagedResult<TModel>
    {
        public IEnumerable<TModel> Items { get; set; }
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalRecords / PageSize) : 0;

        public PagedResult(IEnumerable<TModel> items, int totalRecords, int pageNumber, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
