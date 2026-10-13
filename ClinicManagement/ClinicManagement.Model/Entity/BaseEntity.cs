namespace ClinicManagement.Model.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeleteddBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
