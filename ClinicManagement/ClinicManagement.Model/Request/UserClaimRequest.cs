namespace ClinicManagement.Model.Request
{
	public class UserClaimRequest : BaseRequest
    {
		public Guid? UserId { get; set; }
		public Guid? UserGroupId { get; set; }
	}
}
