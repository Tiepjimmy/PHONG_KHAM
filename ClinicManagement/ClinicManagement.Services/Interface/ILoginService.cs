using ClinicManagement.Model.Request;
using ClinicManagement.Model.Response;

namespace ClinicManagement.Services.Interface
{
    public interface ILoginService
    {
        public Task<Response<AuthenticateResponse>> Authenticate(LoginRequest request);
    }
}
