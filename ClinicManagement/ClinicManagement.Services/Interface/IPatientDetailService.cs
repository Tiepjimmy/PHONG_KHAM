using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels.Custom;

namespace ClinicManagement.Services.Interface
{
    public interface IPatientDetailService
    {
        public Task<IResults<PatientDetailCustomViewModel>> GetAll(PatientDetailSearchRequest request);
        public Task<IResult<bool>> Save(PatientDetailRequest request, string typeSave);
        public IResults<PatientDetail> GetAllData();
        public Task<IResult<PatientDetail>> GetById(Guid id);
        public Task<IResult<bool>> DeleteByPatientId(Guid? patientId);
        public Task<IResult<bool>> SaveAfterPatient(Guid? patientId, List<PatientDetailRequest> listPatientDetail);
    }
}
