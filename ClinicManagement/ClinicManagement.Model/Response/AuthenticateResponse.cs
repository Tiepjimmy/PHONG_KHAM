using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Model.Response
{
    public sealed class AuthenticateResponse : AuthenticateViewModel
    {
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public int ExpireTime { get; set; }
    }
}
