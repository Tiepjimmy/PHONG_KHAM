namespace ClinicManagement.Model.Request
{
	public class AdministrativeWardRequest : BaseRequest
    {
		public string? Code { get; set; }
		public string? CodeName { get; set; }
		public string? NameVN { get; set; }
		public string? NameEN { get; set; }
		public string? FullNameVN { get; set; }
		public string? FullNameEN { get; set; }
		public Guid? AdministrativeDistrictId { get; set; }
		public Guid? AdministrativeUnitId { get; set; }
		public bool IsDefault { get; set; }
	}
}
