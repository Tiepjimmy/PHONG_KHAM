namespace ClinicManagement.Model.Request
{
	public class PatientRequest : BaseRequest
    {
		public long CouponCode { get; set; }
		public string? FullName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Sex { get; set; }
		public string? PhoneNumber { get; set; }
		public Guid? AdministrativeProvinceId { get; set; }
		public Guid? AdministrativeDistrictId { get; set; }
		public Guid? AdministrativeWardId { get; set; }
		public string? Address { get; set; }
		public string? Diagnosis { get; set; }
		public string? Result { get; set; }
		public int Visit { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
