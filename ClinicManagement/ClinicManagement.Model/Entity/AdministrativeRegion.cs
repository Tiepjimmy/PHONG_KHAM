namespace ClinicManagement.Model.Entity
{
	public class AdministrativeRegion : BaseEntity
    {
		public string? CodeVN { get; set; }
		public string? CodeEN { get; set; }
		public string? NameVN { get; set; }
		public string? NameEN { get; set; }
		public bool IsDefault { get; set; }
	}
}
