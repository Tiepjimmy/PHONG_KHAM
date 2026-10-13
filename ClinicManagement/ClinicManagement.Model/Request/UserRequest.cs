namespace ClinicManagement.Model.Request
{
    public class UserRequest : BaseRequest
    {
        public string? UserName { get; set; }
        public Guid? PersonId { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDefault { get; set; }
        public string? ListUserGroupId { get; set; }
    }
}
