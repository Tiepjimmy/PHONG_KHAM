using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request.Custom;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels.Custom;

namespace ClinicManagement.Services.Interface
{
    public interface IPatientService
    {
        public Task<IResults<PatientCustomViewModel>> GetAll(PatientSearchRequest request);
        public Task<IResult<bool>> Save(PatientCustomRequest request, string typeSave);
        public IResults<Patient> GetAllData();
        public Task<IResult<PatientModel>> GetById(Guid id);
        public IResult<long> GenerateCouponCode();
    }
}
