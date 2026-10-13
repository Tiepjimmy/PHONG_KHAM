using Microsoft.AspNetCore.Http;

namespace ClinicManagement.Model.Request
{
    public class FileRequest
    {
        public string? FileName { get; set; }
        public IFormFile? File { get; set; }
    }
}
