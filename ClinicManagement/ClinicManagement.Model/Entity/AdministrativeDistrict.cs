namespace ClinicManagement.Model.Entity
{
	public class AdministrativeDistrict : BaseEntity
    {
		public string? Code { get; set; }
		public string? CodeName { get; set; }
		public string? NameVN { get; set; }
		public string? NameEN { get; set; }
		public string? FullNameVN { get; set; }
		public string? FullNameEN { get; set; }
		public Guid? AdministrativeProvinceId { get; set; }
		public Guid? AdministrativeUnitId { get; set; }
		public bool IsDefault { get; set; }
	}
}
