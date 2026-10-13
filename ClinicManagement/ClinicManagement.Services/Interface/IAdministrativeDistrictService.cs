using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IAdministrativeDistrictService
    {
        public Task<IResults<AdministrativeDistrictViewModel>> GetAll(AdministrativeDistrictSearchRequest request);
        public Task<IResult<bool>> Save(AdministrativeDistrictRequest request, string typeSave);
        public IResults<AdministrativeDistrict> GetAllData();
        public Task<IResult<AdministrativeDistrict>> GetById(Guid id);
        public IResults<AdministrativeDistrict> GetAllDataByAdministrativeProvinceId(Guid? administrativeProvinceId);
    }
}
