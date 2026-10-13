using Microsoft.AspNetCore.Http;

namespace ClinicManagement.Model.Request.Custom
{
    public class TestDemoCustomRequest : TestDemoRequest
    {
        public IFormFile? File { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
