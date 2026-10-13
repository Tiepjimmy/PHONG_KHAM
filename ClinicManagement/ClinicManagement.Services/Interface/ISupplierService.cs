using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
	public interface ISupplierService
    {
		public Task<IResults<SupplierViewModel>> GetAll(SupplierSearchRequest request);
        public Task<IResult<bool>> Save(SupplierRequest request, string typeSave);
        public IResults<Supplier> GetAllData();
        public Task<IResult<Supplier>> GetById(Guid id);
	}
}
