namespace ClinicManagement.Model.SearchRequest
{
    public class UserClaimSearchRequest : BaseSearchRequest
    {
        public Guid? UserId { get; set; }
        public Guid? UserGroupId { get; set; }
    }
}
