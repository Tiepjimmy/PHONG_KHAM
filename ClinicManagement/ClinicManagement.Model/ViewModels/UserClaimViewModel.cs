namespace ClinicManagement.Model.ViewModels
{
	public class UserClaimViewModel : BaseViewModel
    {
		public Guid? UserId { get; set; }
		public string? UserName { get; set; }
		public Guid? UserGroupId { get; set; }
		public string? UserGroupName { get; set; }
	}
}
