using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
	public interface IAdministrativeRegionService
    {
		public Task<IResults<AdministrativeRegionViewModel>> GetAll(AdministrativeRegionSearchRequest request);
        public Task<IResult<bool>> Save(AdministrativeRegionRequest request, string typeSave);
        public IResults<AdministrativeRegion> GetAllData();
        public Task<IResult<AdministrativeRegion>> GetById(Guid id);
	}
}
