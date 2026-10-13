using AutoMapper;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Services.Interface;
using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using NetTopologySuite.Algorithm;
using Serilog;
using System.Drawing;
using System.Net;


namespace ClinicManagement.Services.Implement
{
    public class CustomerService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IAuditChangeService auditChangeService) : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;

        public async Task<IResults<CustomerViewModel>> GetAll(CustomerSearchRequest request)
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
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<CustomerViewModel>("uspCustomer_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<CustomerViewModel>
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

        public IResults<Customer> GetAllData()
        {
            try
            {
                var list = _unitOfWork.CustomerRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<Customer>
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

        public IResults<Customer> GetAllDataByAreaId(Guid? areaId)
        {
            try
            {
                if (areaId == null)
                {
                    return new Results<Customer>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.AreaId == areaId).ToList();
                return new Results<Customer>
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

        public IResults<Customer> GetAllDataByBranchId(Guid? branchId)
        {
            try
            {
                if (branchId == null)
                {
                    return new Results<Customer>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.BranchId == branchId).ToList();
                return new Results<Customer>
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

        public IResults<Customer> GetAllDataByDepartmentId(Guid? departmentId)
        {
            try
            {
                if (departmentId == null)
                {
                    return new Results<Customer>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.DepartmentId == departmentId).ToList();
                return new Results<Customer>
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

        public IResults<Customer> GetAllDataByRegionId(Guid? regionId)
        {
            try
            {
                if (regionId == null)
                {
                    return new Results<Customer>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.RegionId == regionId).ToList();
                return new Results<Customer>
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

        public IResults<Customer> GetAllDataByUserAssignId(Guid? userAssignId)
        {
            try
            {
                if (userAssignId == null)
                {
                    return new Results<Customer>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                        ListData = [],
                        TotalRecords = 0
                    };
                }
                var list = GetAllData().ListData.Where(x => x.UserAssignId == userAssignId).ToList();
                return new Results<Customer>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByUserAssignId: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<Customer>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.CustomerRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<Customer>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                return new Result<Customer>
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

        private async Task<string> Insert(CustomerRequest request, string typeSave)
        {
            var checkDuplicate = _unitOfWork.CustomerRepository.GetSingle(x => x.PhoneNumber == request.PhoneNumber && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.CodeExist, request.PhoneNumber);
            }
            var insert = _mapper.Map<Customer>(request);
            _unitOfWork.CustomerRepository.Add(insert);
            await _unitOfWork.CustomerRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Update(CustomerRequest request, string typeSave)
        {
            var exist = _unitOfWork.CustomerRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkDuplicate = _unitOfWork.CustomerRepository.GetSingle(x => x.Id != request.Id && x.PhoneNumber == request.PhoneNumber && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.CodeExist, request.PhoneNumber);
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.CustomerRepository.Update(update);
            await _unitOfWork.CustomerRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkForeignKey = (await _auditChangeService.GetColumnNameForeignKey()).ListData.Where(x => (x.ColumnName ?? string.Empty).Contains(nameof(Customer))).ToList();
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
            var delete = _unitOfWork.CustomerRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.CustomerRepository.Update(delete);
            await _unitOfWork.CustomerRepository.CommitAsync(typeSave);
            return string.Empty;
        }
        public async Task<IResult<bool>> Save(CustomerRequest request, string typeSave)
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
