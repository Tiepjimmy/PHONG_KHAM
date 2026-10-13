using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
	public interface IAdministrativeUnitService
    {
		public Task<IResults<AdministrativeUnitViewModel>> GetAll(AdministrativeUnitSearchRequest request);
        public Task<IResult<bool>> Save(AdministrativeUnitRequest request, string typeSave);
        public IResults<AdministrativeUnit> GetAllData();
        public Task<IResult<AdministrativeUnit>> GetById(Guid id);
	}
}
