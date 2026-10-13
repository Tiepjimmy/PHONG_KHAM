namespace ClinicManagement.Model.Entity
{
	public class AdministrativeProvince : BaseEntity
    {
		public string? Code { get; set; }
		public string? CodeName { get; set; }
		public string? NameVN { get; set; }
		public string? NameEN { get; set; }
		public string? FullNameVN { get; set; }
		public string? FullNameEN { get; set; }
		public Guid? AdministrativeUnitId { get; set; }
		public Guid? AdministrativeRegionId { get; set; }
		public bool IsDefault { get; set; }
	}
}
