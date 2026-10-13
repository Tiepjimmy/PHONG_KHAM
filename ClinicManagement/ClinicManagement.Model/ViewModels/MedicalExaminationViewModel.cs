namespace ClinicManagement.Model.ViewModels
{
	public class MedicalExaminationViewModel : BaseViewModel
    {
		public string? Name { get; set; }
		public decimal Price { get; set; }
		public string? NormalIndex { get; set; }
	}
}
