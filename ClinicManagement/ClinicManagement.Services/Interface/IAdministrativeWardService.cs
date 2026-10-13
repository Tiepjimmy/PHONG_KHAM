using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IAdministrativeWardService
    {
        public Task<IResults<AdministrativeWardViewModel>> GetAll(AdministrativeWardSearchRequest request);
        public Task<IResult<bool>> Save(AdministrativeWardRequest request, string typeSave);
        public IResults<AdministrativeWard> GetAllData();
        public Task<IResult<AdministrativeWard>> GetById(Guid id);
        public IResults<AdministrativeWard> GetAllDataByAdministrativeDistrictId(Guid? administrativeDistrictId);
    }
}
