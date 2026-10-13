namespace ClinicManagement.Model.Response
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class IpLocationResponse
    {
        public string? ip { get; set; }
        public int response_code { get; set; }
        public string? response_message { get; set; }
    }
}
