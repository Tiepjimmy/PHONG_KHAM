namespace ClinicManagement.Model.SearchRequest
{
    public class BaseSearchRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
        public string? SearchText { get; set; }
    }
}
