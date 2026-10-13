namespace ClinicManagement.Model.SearchRequest
{
    public class PatientSearchRequest : BaseSearchRequest
    {
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
