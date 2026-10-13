namespace ClinicManagement.Model.Entity
{
	public class UserClaim : BaseEntity
    {
		public Guid? UserId { get; set; }
		public Guid? UserGroupId { get; set; }
	}
}
