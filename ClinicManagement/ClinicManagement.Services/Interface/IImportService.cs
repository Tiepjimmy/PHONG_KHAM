using Microsoft.AspNetCore.Http;
using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.Response;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IImportService
    {
        public Task<IResult<bool>> Save(ImportExcelRequest request);
        public UploadFileResponse UploadFileForInsertUpdate(List<IFormFile> file, string type);
        public InformationFileViewModel GetInformationFile(IFormFile file);
        public bool UploadFile(List<FileRequest> listFile, string type);
    }
}
