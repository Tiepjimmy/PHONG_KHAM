using Microsoft.AspNetCore.Http;

namespace ClinicManagement.Model.Request.Custom
{
    public class PersonCustomRequest : PersonRequest
    {
        public IFormFile? File { get; set; }
    }
}
