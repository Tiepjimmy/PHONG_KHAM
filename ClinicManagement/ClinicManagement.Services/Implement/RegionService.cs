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
using Serilog;
using System.Net;


namespace ClinicManagement.Services.Implement
{
    public class RegionService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IAuditChangeService auditChangeService) : IRegionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;
        public async Task<IResults<RegionViewModel>> GetAll(RegionSearchRequest request)
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
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<RegionViewModel>("uspRegion_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<RegionViewModel>
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

        public IResults<Region> GetAllData()
        {
            try
            {
                var list = _unitOfWork.RegionRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<Region>
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

        public async Task<IResult<Region>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.RegionRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<Region>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                return new Result<Region>
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
        private async Task<string> Insert(RegionRequest request, string typeSave)
        {
            var checkDuplicate = _unitOfWork.RegionRepository.GetSingle(x => x.Code == request.Code && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.CodeExist, request.Code);
            }
            var insert = _mapper.Map<Region>(request);
            _unitOfWork.RegionRepository.Add(insert);
            await _unitOfWork.RegionRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Update(RegionRequest request, string typeSave)
        {
            var exist = _unitOfWork.RegionRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkDuplicate = _unitOfWork.RegionRepository.GetSingle(x => x.Id != request.Id && x.Code == request.Code && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.CodeExist, request.Code);
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.RegionRepository.Update(update);
            await _unitOfWork.RegionRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkForeignKey = (await _auditChangeService.GetColumnNameForeignKey()).ListData.Where(x => (x.ColumnName ?? string.Empty).Contains(nameof(Region))).ToList();
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
            var delete = _unitOfWork.RegionRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.RegionRepository.Update(delete);
            await _unitOfWork.RegionRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        public async Task<IResult<bool>> Save(RegionRequest request, string typeSave)
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
