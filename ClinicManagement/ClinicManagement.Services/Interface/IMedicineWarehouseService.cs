using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.Response;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using GemBox.Document;

namespace ClinicManagement.Services.Interface
{
    public interface IMedicineWarehouseService
    {
        public Task<IResults<MedicineWarehouseViewModel>> GetAll(MedicineWarehouseSearchRequest request);
        public Task<IResult<bool>> Save(MedicineWarehouseRequest request, string typeSave);
        public IResults<MedicineWarehouse> GetAllData();
        public Task<IResult<MedicineWarehouse>> GetById(Guid id);
        public Task<IResults<MedicineWarehouseViewModel>> GetExpiry(MedicineWarehouseSearchRequest request);
        public Task<ExportToExcelResponse> DownloadExpiry(MedicineWarehouseSearchRequest request, DocumentModel document);
    }
}
