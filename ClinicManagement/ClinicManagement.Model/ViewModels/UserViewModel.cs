namespace ClinicManagement.Model.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? UserGroupName { get; set; }
        public Guid? PersonId { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDefault { get; set; }
    }
}
