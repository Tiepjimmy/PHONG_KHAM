namespace ClinicManagement.Model.SearchRequest
{
    public class UserSearchRequest : BaseSearchRequest
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? UserGroupId { get; set; }
    }
}
