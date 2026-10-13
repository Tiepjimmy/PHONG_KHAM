namespace ClinicManagement.Model.Entity
{
	public class MedicalExamination : BaseEntity
    {
		public string? Name { get; set; }
		public decimal Price { get; set; }
		public string? NormalIndex { get; set; }
	}
}
