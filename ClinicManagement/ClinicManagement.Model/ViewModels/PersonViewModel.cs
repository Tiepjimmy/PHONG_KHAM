namespace ClinicManagement.Model.ViewModels
{
	public class PersonViewModel : BaseViewModel
    {
		public string? FullName { get; set; }
		public string? Avatar { get; set; }
		public string? PhoneNumber { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Sex { get; set; }
		public string? Address { get; set; }
	}
}
