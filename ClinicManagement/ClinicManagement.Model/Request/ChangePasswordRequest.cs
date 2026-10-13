namespace ClinicManagement.Model.Request
{
    public class ChangePasswordRequest : BaseRequest
    {
        public string? PassWordOld { get; set; }
        public string? PassWordNew { get; set; }
    }
}
