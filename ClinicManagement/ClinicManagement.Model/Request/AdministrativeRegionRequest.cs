namespace ClinicManagement.Model.Request
{
	public class AdministrativeRegionRequest : BaseRequest
    {
		public string? CodeVN { get; set; }
		public string? CodeEN { get; set; }
		public string? NameVN { get; set; }
		public string? NameEN { get; set; }
		public bool IsDefault { get; set; }
	}
}
