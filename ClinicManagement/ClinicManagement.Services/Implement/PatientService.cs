using AutoMapper;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.Request.Custom;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Model.ViewModels.Custom;
using ClinicManagement.Services.Interface;
using Dapper;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class PatientService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IAuditChangeService auditChangeService, IPatientDetailService patientDetailService
        , IMedicalExaminationService medicalExaminationService, IPersonService personService) : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;
        private readonly IPatientDetailService _patientDetailService = patientDetailService;
        private readonly IMedicalExaminationService _medicalExaminationService = medicalExaminationService;
        private readonly IPersonService _personService = personService;

        public async Task<IResults<PatientCustomViewModel>> GetAll(PatientSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@SearchText", request.SearchText);
                dynamicParameters.Add("@PhoneNumber", request.PhoneNumber);
                dynamicParameters.Add("@CreatedDate", request.CreatedDate);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<PatientCustomViewModel>("uspPatient_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<PatientCustomViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAll: ", ex.ToString()));
                throw;
            }
        }

        public IResults<Patient> GetAllData()
        {
            try
            {
                var list = _unitOfWork.PatientRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<Patient>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllData: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<bool>> Save(PatientCustomRequest request, string typeSave)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var result = string.Empty;
                switch (typeSave)
                {
                    case ConstantHelper.TypeSave.Insert:
                        result = await Insert(request, typeSave);
                        break;
                    case ConstantHelper.TypeSave.Update:
                        result = await Update(request, typeSave);
                        break;
                    case ConstantHelper.TypeSave.Delete:
                        result = await Delete(request.Id, typeSave);
                        break;
                    default:
                        break;
                }
                _unitOfWork.CommitTransaction();
                if (!string.IsNullOrEmpty(result))
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = result,
                        Data = false
                    };
                }
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = typeSave == ConstantHelper.TypeSave.Delete ? ConstantHelper.Message.DeleteSuccess : ConstantHelper.Message.SaveSuccess,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Save: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        private async Task<string> Insert(PatientCustomRequest request, string typeSave)
        {
            var insert = _mapper.Map<Patient>(request);
            _unitOfWork.PatientRepository.Add(insert);
            await _unitOfWork.PatientRepository.CommitAsync(typeSave);
            var listPatientDetail = JsonConvert.DeserializeObject<List<PatientDetailRequest>>(request.ListPatientDetail ?? string.Empty);
            await _patientDetailService.SaveAfterPatient(insert.Id, listPatientDetail ?? []);
            return string.Empty;
        }

        private async Task<string> Update(PatientCustomRequest request, string typeSave)
        {
            var exist = _unitOfWork.PatientRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.PatientRepository.Update(update);
            await _unitOfWork.PatientRepository.CommitAsync(typeSave);
            var listPatientDetail = JsonConvert.DeserializeObject<List<PatientDetailRequest>>(request.ListPatientDetail ?? string.Empty);
            await _patientDetailService.SaveAfterPatient(update.Id, listPatientDetail ?? []);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var delete = _unitOfWork.PatientRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.PatientRepository.Update(delete);
            await _unitOfWork.PatientRepository.CommitAsync(typeSave);
            await _patientDetailService.DeleteByPatientId(id);
            return string.Empty;
        }

        public async Task<IResult<PatientModel>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.PatientRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<PatientModel>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                PatientModel result = new()
                {
                    Id = data.Id,
                    CouponCode = data.CouponCode,
                    FullName = data.FullName,
                    DateOfBirth = data.DateOfBirth,
                    Sex = data.Sex,
                    PhoneNumber = data.PhoneNumber,
                    AdministrativeProvinceId = data.AdministrativeProvinceId,
                    AdministrativeDistrictId = data.AdministrativeDistrictId,
                    AdministrativeWardId = data.AdministrativeWardId,
                    Address = data.Address,
                    Diagnosis = data.Diagnosis,
                    Result = data.Result,
                    Visit = data.Visit,
                    TotalPrice = data.TotalPrice,
                    ListPatientDetail = _patientDetailService.GetAllData().ListData
                        .Where(x => x.PatientId == id)
                        .Select(x => new PatientDetailViewModel()
                        {
                            Id = x.Id,
                            MedicalExaminationId = x.MedicalExaminationId,
                            MedicalExaminationName = _medicalExaminationService.GetById(x.MedicalExaminationId ?? new()).GetAwaiter().GetResult().Data?.Name,
                            PersonId = x.PersonId,
                            PersonName = _personService.GetById(x.PersonId ?? new()).GetAwaiter().GetResult().Data?.FullName,
                            Price = x.Price,
                            Quantity = x.Quantity,
                            TotalPrice = x.TotalPrice,
                            Result = x.Result,
                        }).ToList(),
                };
                return new Result<PatientModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetById: ", ex.ToString()));
                throw;
            }
        }

        public IResult<long> GenerateCouponCode()
        {
            try
            {
                var couponCode = _unitOfWork.PatientRepository.GetAll().OrderByDescending(x => x.CouponCode).FirstOrDefault()?.CouponCode ?? 0;
                return new Result<long>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = couponCode + 1,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GenerateCouponCode: ", ex.ToString()));
                throw;
            }
        }
    }
}
