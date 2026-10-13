namespace ClinicManagement.Model.ViewModels
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string? UserName { get; }
        public Guid? PersonId { get; set; }
    }
}
