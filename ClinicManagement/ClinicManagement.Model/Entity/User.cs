namespace ClinicManagement.Model.Entity
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
        public Guid? PersonId { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDefault { get; set; }
    }
}
