using GemBox.Document;
using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request.Custom;
using ClinicManagement.Model.Response;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface ITestDemoService
    {
        public Task<IResults<TestDemoViewModel>> GetAll(TestDemoSearchRequest request);
        public Task<IResult<bool>> Save(TestDemoCustomRequest request, string typeSave);
        public IResults<TestDemo> GetAllData();
        public Task<IResult<TestDemoModel>> GetById(Guid id);
        public Task<ExportToExcelResponse> ExportToExcel(TestDemoSearchRequest request);
        public ExportToExcelResponse DownloadToWord(DocumentModel document);
    }
}
