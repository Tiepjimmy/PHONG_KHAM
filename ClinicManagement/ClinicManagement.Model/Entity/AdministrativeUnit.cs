namespace ClinicManagement.Model.Entity
{
	public class AdministrativeUnit : BaseEntity
    {
		public string? CodeNameVN { get; set; }
		public string? CodeNameEN { get; set; }
		public string? FullNameVN { get; set; }
		public string? FullNameEN { get; set; }
		public string? ShortNameVN { get; set; }
		public string? ShortNameEN { get; set; }
		public bool IsDefault { get; set; }
	}
}
