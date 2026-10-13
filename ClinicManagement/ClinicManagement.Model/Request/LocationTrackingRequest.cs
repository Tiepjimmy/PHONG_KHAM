namespace ClinicManagement.Model.Request
{
    public class LocationTrackingRequest : BaseRequest
    {
        public string? IPv4 { get; set; }
        public string? PcName { get; set; }
    }
}
