using AutoMapper;
using Dapper;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class UserClaimService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IAuditChangeService auditChangeService) : IUserClaimService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;

        public async Task<IResults<UserClaimViewModel>> GetAll(UserClaimSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@UserId", request.UserId);
                dynamicParameters.Add("@UserGroupId", request.UserGroupId);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<UserClaimViewModel>("uspUserClaim_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<UserClaimViewModel>
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

        public IResults<UserClaim> GetAllData()
        {
            try
            {
                var list = _unitOfWork.UserClaimRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<UserClaim>
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

        public async Task<IResult<bool>> Save(UserClaimRequest request, string typeSave)
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

        private async Task<string> Insert(UserClaimRequest request, string typeSave)
        {
            var insert = _mapper.Map<UserClaim>(request);
            _unitOfWork.UserClaimRepository.Add(insert);
            await _unitOfWork.UserClaimRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Update(UserClaimRequest request, string typeSave)
        {
            var exist = _unitOfWork.UserClaimRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.UserClaimRepository.Update(update);
            await _unitOfWork.UserClaimRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkForeignKey = (await _auditChangeService.GetColumnNameForeignKey()).ListData.Where(x => (x.ColumnName ?? string.Empty).Contains(nameof(UserClaim))).ToList();
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
            var delete = _unitOfWork.UserClaimRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.UserClaimRepository.Update(delete);
            await _unitOfWork.UserClaimRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        public async Task<IResult<UserClaim>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.UserClaimRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<UserClaim>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                return new Result<UserClaim>
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

        public async Task<IResult<bool>> SaveAfterUser(Guid userId, string? listUserGroupId)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                List<UserClaim> listUserClaim = GetAllData().ListData.Where(x => x.UserId == userId).ToList();
                //Delete
                if (listUserClaim.Count != 0)
                {
                    foreach (var item in listUserClaim)
                    {
                        _unitOfWork.UserClaimRepository.Update(item);
                    }
                    await _unitOfWork.UserClaimRepository.CommitAsync(ConstantHelper.TypeSave.Delete);
                }
                if (!string.IsNullOrEmpty(listUserGroupId))
                {
                    List<string> subs = [.. listUserGroupId.Split(ConstantHelper.Comma)];
                    List<string> listId = listUserClaim.Select(x => x.UserGroupId.ToString() ?? string.Empty).ToList();

                    //Insert when not exist
                    List<string> subNotExist = subs.Where(x => !listId.Contains(x)).ToList();
                    foreach (var sub in subNotExist)
                    {
                        UserClaim model = new()
                        {
                            Id = Guid.NewGuid(),
                            UserId = userId,
                            UserGroupId = Guid.Parse(sub)
                        };
                        _unitOfWork.UserClaimRepository.Add(model);
                    }
                    await _unitOfWork.UserClaimRepository.CommitAsync(ConstantHelper.TypeSave.Insert);

                    //Update when exist
                    List<string> subExist = subs.Where(x => listId.Contains(x)).ToList();
                    foreach (var sub in subExist)
                    {
                        var userClaim = listUserClaim.FirstOrDefault(x => x.UserGroupId.ToString() == sub);
                        if (userClaim != null)
                        {
                            userClaim.DeletedDate = null;
                            userClaim.DeleteddBy = null;
                            userClaim.IsDeleted = false;
                            _unitOfWork.UserClaimRepository.Update(userClaim);
                        }
                    }
                    await _unitOfWork.UserClaimRepository.CommitAsync(ConstantHelper.TypeSave.Update);
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
                Log.Error(string.Concat("[", GetType().Name, "]", " SaveAfterUser: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
