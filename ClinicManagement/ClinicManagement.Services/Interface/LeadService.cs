using AutoMapper;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Services.Interface
{
     public class LeadService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IAuditChangeService auditChangeService) : ILeadService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;
        public async Task<IResults<LeadViewModel>> GetAll(LeadSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@SearchText", request.SearchText);
                //dynamicParameters.Add("@AdministrativeProvinceId", request.AdministrativeProvinceId);
                //dynamicParameters.Add("@AdministrativeUnitId", request.AdministrativeUnitId);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<LeadViewModel>("uspLead_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<LeadViewModel>
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

        public IResults<Leads> GetAllData()
        {
            try
            {
                var list = _unitOfWork.LeadRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<Leads>
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

        public IResults<Leads> GetAllDataByAreaId(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new Results<Leads>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.AreaId == id).ToList();
                return new Results<Leads>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByAreaId: ", ex.ToString()));
                throw;
            }
        }

        public IResults<Leads> GetAllDataByBranchId(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new Results<Leads>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.BranchId == id).ToList();
                return new Results<Leads>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByBranchId: ", ex.ToString()));
                throw;
            }
        }

        public IResults<Leads> GetAllDataByCustomerId(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new Results<Leads>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.CustomerId == id).ToList();
                return new Results<Leads>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByCustomerId: ", ex.ToString()));
                throw;
            }
        }

        public IResults<Leads> GetAllDataByDepartmentId(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new Results<Leads>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.DepartmentId == id).ToList();
                return new Results<Leads>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByDepartmentId: ", ex.ToString()));
                throw;
            }
        }

        public IResults<Leads> GetAllDataByRegionId(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new Results<Leads>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.RegionId == id).ToList();
                return new Results<Leads>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByRegionId: ", ex.ToString()));
                throw;
            }
        }

        public IResults<Leads> GetAllDataByUserId(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new Results<Leads>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.UserId == id).ToList();
                return new Results<Leads>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByUserId: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<Leads>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.LeadRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<Leads>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                return new Result<Leads>
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
        private async Task<string> Insert(LeadRequest request, string typeSave)
        {
            var checkDuplicate = _unitOfWork.LeadRepository.GetSingle(x => x.Code == request.Code && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.CodeExist, request.Code);
            }
            var insert = _mapper.Map<Leads>(request);
            _unitOfWork.LeadRepository.Add(insert);
            await _unitOfWork.LeadRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Update(LeadRequest request, string typeSave)
        {
            var exist = _unitOfWork.LeadRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkDuplicate = _unitOfWork.LeadRepository.GetSingle(x => x.Id != request.Id && x.Code == request.Code && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.CodeExist, request.Code);
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.LeadRepository.Update(update);
            await _unitOfWork.LeadRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkForeignKey = (await _auditChangeService.GetColumnNameForeignKey()).ListData.Where(x => (x.ColumnName ?? string.Empty).Contains(nameof(Leads))).ToList();
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
            var delete = _unitOfWork.LeadRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.LeadRepository.Update(delete);
            await _unitOfWork.LeadRepository.CommitAsync(typeSave);
            return string.Empty;
        }
        public async Task<IResult<bool>> Save(LeadRequest request, string typeSave)
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
    }
}
