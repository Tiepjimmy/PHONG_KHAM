using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
	public interface IAdministrativeProvinceService
    {
		public Task<IResults<AdministrativeProvinceViewModel>> GetAll(AdministrativeProvinceSearchRequest request);
        public Task<IResult<bool>> Save(AdministrativeProvinceRequest request, string typeSave);
        public IResults<AdministrativeProvince> GetAllData();
        public Task<IResult<AdministrativeProvince>> GetById(Guid id);
	}
}
