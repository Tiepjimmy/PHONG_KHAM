using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request.Custom;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Model.ViewModels.Custom;

namespace ClinicManagement.Services.Interface
{
    public interface IPersonService
    {
        public Task<IResults<PersonViewModel>> GetAll(PersonSearchRequest request);
        public Task<IResult<bool>> Save(PersonCustomRequest request, string typeSave);
        public IResults<Person> GetAllData();
        public Task<IResult<Person>> GetById(Guid id);
        public Task<IResults<PersonViewModel>> GetAllDataByKey(string key, bool isEdit);
        public IResult<PersonCustomViewModel> GetProfile();
        public Task<IResult<bool>> UpdateProfile(PersonCustomRequest request);
    }
}
