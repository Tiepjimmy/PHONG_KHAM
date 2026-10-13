namespace ClinicManagement.Model.Request
{
	public class MedicalExaminationRequest : BaseRequest
    {
		public string? Name { get; set; }
		public decimal Price { get; set; }
		public string? NormalIndex { get; set; }
	}
}
