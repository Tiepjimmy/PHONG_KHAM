using ClinicManagement.Model.Entity;

namespace ClinicManagement.Model.Model
{
    public class UserModel : User
    {
        public string? FullName { get; set; }
        public string? ListUserGroupId { get; set; }
    }
}
