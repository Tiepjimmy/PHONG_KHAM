using AutoMapper;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Data.Share;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels.Custom;
using ClinicManagement.Services.Interface;
using Dapper;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class PatientDetailService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IAuditChangeService auditChangeService, IAuthenticatedService authenticatedService) : IPatientDetailService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;
        private readonly IAuthenticatedService _authenticatedService = authenticatedService;

        public async Task<IResults<PatientDetailCustomViewModel>> GetAll(PatientDetailSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@SearchText", request.SearchText);
                dynamicParameters.Add("@PhoneNumber", request.PhoneNumber);
                dynamicParameters.Add("@CreatedDate", request.CreatedDate);
                dynamicParameters.Add("@PersonId", _authenticatedService.UserInfo.PersonId);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<PatientDetailCustomViewModel>("uspPatientDetail_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<PatientDetailCustomViewModel>
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

        public IResults<PatientDetail> GetAllData()
        {
            try
            {
                var list = _unitOfWork.PatientDetailRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<PatientDetail>
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

        public async Task<IResult<bool>> Save(PatientDetailRequest request, string typeSave)
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
                    case ConstantHelper.TypeSave.UpdateResult:
                        result = await UpdateResult(request, ConstantHelper.TypeSave.Update);
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

        private async Task<string> Insert(PatientDetailRequest request, string typeSave)
        {
            var insert = _mapper.Map<PatientDetail>(request);
            _unitOfWork.PatientDetailRepository.Add(insert);
            await _unitOfWork.PatientDetailRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Update(PatientDetailRequest request, string typeSave)
        {
            var exist = _unitOfWork.PatientDetailRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.PatientDetailRepository.Update(update);
            await _unitOfWork.PatientDetailRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkForeignKey = (await _auditChangeService.GetColumnNameForeignKey()).ListData.Where(x => (x.ColumnName ?? string.Empty).Contains(nameof(PatientDetail))).ToList();
            if (checkForeignKey.Count != 0)
            {
                foreach (var item in checkForeignKey)
                {
                    var strSql = string.Concat("SELECT COUNT(*) FROM ", item.TableName, " where ", item.ColumnName, " = '", id, "' AND IsDeleted = 0");
                    DynamicParameters dynamicParameters = new();
                    dynamicParameters.Add("@StrSql", strSql);
                    var checkExist = _dapperRepository.QueryFirstOrDefaultUsingStoreProc<long>("uspSystem_ExecuteQuery", dynamicParameters);
                    if (checkExist > 0)
                    {
                        return string.Format(ConstantHelper.Message.DeleteExistFail);
                    }
                }
            }
            var delete = _unitOfWork.PatientDetailRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.PatientDetailRepository.Update(delete);
            await _unitOfWork.PatientDetailRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        public async Task<IResult<PatientDetail>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.PatientDetailRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<PatientDetail>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                return new Result<PatientDetail>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = data,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetById: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<bool>> DeleteByPatientId(Guid? patientId)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var list = GetAllData().ListData.Where(x => x.PatientId == patientId).ToList();
                if (list.Count != 0)
                {
                    foreach (var item in list)
                    {
                        _unitOfWork.PatientDetailRepository.Update(item);
                    }
                    await _unitOfWork.PatientDetailRepository.CommitAsync(ConstantHelper.TypeSave.Delete);
                }
                _unitOfWork.CommitTransaction();
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.SaveSuccess,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " DeleteByPatientId: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IResult<bool>> SaveAfterPatient(Guid? patientId, List<PatientDetailRequest> listPatientDetail)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                List<PatientDetail> listInsert = [];
                List<PatientDetail> listUpdate = [];
                foreach (var item in listPatientDetail)
                {
                    item.PatientId = patientId;
                    var data = (await GetById(item.Id ?? new())).Data;
                    if (data != null)
                    {
                        var update = _mapper.Map(item, data);
                        listUpdate.Add(update);
                    }
                    else
                    {
                        var insert = _mapper.Map<PatientDetail>(item);
                        listInsert.Add(insert);
                    }
                }
                var listId = listPatientDetail.Select(x => x.Id.ToString()).ToList();
                List<PatientDetail> listDelete = GetAllData().ListData.Where(x => !listId.Contains(x.Id.ToString())).ToList();
                //insert
                foreach (var item in listInsert)
                {
                    _unitOfWork.PatientDetailRepository.Add(item);
                }
                await _unitOfWork.PatientDetailRepository.CommitAsync(ConstantHelper.TypeSave.Insert);
                //update
                foreach (var item in listUpdate)
                {
                    _unitOfWork.PatientDetailRepository.Update(item);
                }
                await _unitOfWork.PatientDetailRepository.CommitAsync(ConstantHelper.TypeSave.Update);
                //delete
                foreach (var item in listDelete)
                {
                    _unitOfWork.PatientDetailRepository.Update(item);
                }
                await _unitOfWork.PatientDetailRepository.CommitAsync(ConstantHelper.TypeSave.Delete);
                _unitOfWork.CommitTransaction();
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.SaveSuccess,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " SaveAfterPatient: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        private async Task<string> UpdateResult(PatientDetailRequest request, string typeSave)
        {
            var exist = _unitOfWork.PatientDetailRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            exist.Result = request.Result;
            _unitOfWork.PatientDetailRepository.Update(exist);
            await _unitOfWork.PatientDetailRepository.CommitAsync(typeSave);
            return string.Empty;
        }
    }
}
