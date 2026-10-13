namespace ClinicManagement.Model.SearchRequest
{
    public class PatientDetailSearchRequest : BaseSearchRequest
    {
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
