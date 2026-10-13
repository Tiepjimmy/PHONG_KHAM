namespace ClinicManagement.Model.Entity
{
	public class PatientDetail : BaseEntity
    {
		public Guid? PatientId { get; set; }
		public Guid? MedicalExaminationId { get; set; }
		public Guid? PersonId { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }
		public string? Result { get; set; }
	}
}
