namespace ClinicManagement.Model.ViewModels
{
	public class AdministrativeProvinceViewModel : BaseViewModel
    {
		public string? Code { get; set; }
		public string? CodeName { get; set; }
		public string? NameVN { get; set; }
		public string? NameEN { get; set; }
		public string? FullNameVN { get; set; }
		public string? FullNameEN { get; set; }
		public Guid? AdministrativeUnitId { get; set; }
		public string? AdministrativeUnitName { get; set; }
		public Guid? AdministrativeRegionId { get; set; }
		public string? AdministrativeRegionName { get; set; }
		public bool IsDefault { get; set; }
	}
}
