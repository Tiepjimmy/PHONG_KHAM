using Microsoft.AspNetCore.Http;
using ClinicManagement.Model.Model;

namespace ClinicManagement.Model.Helper
{
    public static class FormFileHelper
    {
        public static List<IFormFile> ConvertListBase64ToListIFormFile(List<Base64Model> fileBase64s)
        {
            List<IFormFile> formFiles = [];
            foreach (var file in fileBase64s)
            {
                formFiles.Add(CreateFormFileFromBase64(file.FileName ?? string.Empty, file.Base64 ?? string.Empty));
            }
            return formFiles;
        }

        public static IFormFile ConvertBase64ToIFormFile(Base64Model fileBase64)
        {
            return CreateFormFileFromBase64(fileBase64.FileName ?? string.Empty, fileBase64.Base64 ?? string.Empty);
        }

        private static FormFile CreateFormFileFromBase64(string fileName, string base64)
        {
            MemoryStream stream = new();
            var bytes = Convert.FromBase64String(base64);
            stream.Write(bytes);
            stream.Position = 0;
            return new FormFile(stream, 0, stream.Length, "file", fileName);
        }
    }
}
