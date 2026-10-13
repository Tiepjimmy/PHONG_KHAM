namespace ClinicManagement.Model.Request
{
	public class TestDemoRequest : BaseRequest
    {
		public string? FieldString { get; set; }
		public DateTime? FieldDateTime { get; set; }
		public Guid? FieldDropdownId { get; set; }
		public int? FieldNumber { get; set; }
		public string? FieldTextArea { get; set; }
		public string? FieldImageSingle { get; set; }
		public bool? FieldCheckBox { get; set; }
		public bool? FieldSwitch { get; set; }
	}
}
