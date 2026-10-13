namespace ClinicManagement.Model.ViewModels
{
    public class AuthenticateViewModel
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? UserGroupName { get; set; }
        public bool IsAdmin { get; set; }
        public List<UserRoleViewModel>? ListMenu { get; set; }
    }
}
