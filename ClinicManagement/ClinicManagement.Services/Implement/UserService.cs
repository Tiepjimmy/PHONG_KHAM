using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class UserService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IOptions<AppSettingHelper> options
        , IImportService importService, IMapper mapper, IUserClaimService userClaimService
        , IUserGroupService userGroupService, IAuditChangeService auditChangeService) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IImportService _importService = importService;
        private readonly AppSettingHelper _appSettingHelper = options.Value;
        private readonly IMapper _mapper = mapper;
        private readonly IUserClaimService _userClaimService = userClaimService;
        private readonly IUserGroupService _userGroupService = userGroupService;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;

        public async Task<IResults<UserViewModel>> GetAll(UserSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@FullName", request.FullName);
                dynamicParameters.Add("@UserName", request.UserName);
                dynamicParameters.Add("@UserGroupId", request.UserGroupId);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<UserViewModel>("uspUser_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    List<Guid> listUserId = list.Select(x => x.Id).ToList();
                    List<UserClaim> listUserClaim = _userClaimService.GetAllData().ListData.Where(x => listUserId.Contains(x.UserId ?? Guid.NewGuid())).ToList();
                    List<Guid?> listUserGroupId = listUserClaim.Select(x => x.UserGroupId).ToList();
                    List<UserGroup> listUserGroup = _userGroupService.GetAllData().ListData.Where(x => listUserGroupId.Contains(x.Id)).ToList();
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                        x.UserGroupName = string.Join(ConstantHelper.CommaSpace, listUserGroup.Where(e => listUserClaim.Where(d => d.UserId == x.Id).Select(d => d.UserGroupId).Contains(e.Id)).Select(e => e.Name));
                    });
                }
                return new Results<UserViewModel>
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

        public IResults<User> GetAllData()
        {
            try
            {
                var list = _unitOfWork.UserRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<User>
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

        public async Task<IResult<bool>> Save(UserRequest request, string typeSave)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var result = string.Empty;
                User oldValue = new();
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

        private async Task<string> Insert(UserRequest request, string typeSave)
        {
            request.UserName = request.UserName?.ToLower();
            var checkDuplicate = _unitOfWork.UserRepository.GetSingle(x => x.UserName == request.UserName && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.UserNameExist, request.UserName);
            }
            var insert = _mapper.Map<User>(request);
            insert.PassWord = EncriptFunctionsHelper.GeneratePassword(request.UserName ?? string.Empty);
            _unitOfWork.UserRepository.Add(insert);
            await _unitOfWork.UserRepository.CommitAsync(typeSave);
            await _userClaimService.SaveAfterUser(insert.Id, request.ListUserGroupId);
            return string.Empty;
        }

        private async Task<string> Update(UserRequest request, string typeSave)
        {
            var update = _unitOfWork.UserRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || update == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            update.PersonId = request.PersonId;
            update.IsActive = request.IsActive;
            update.IsAdmin = request.IsAdmin;
            _unitOfWork.UserRepository.Update(update);
            await _unitOfWork.UserRepository.CommitAsync(typeSave);
            await _userClaimService.SaveAfterUser(update.Id, request.ListUserGroupId);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var listTable = (await _auditChangeService.GetAllEntityName()).ListData.Where(x => x.EntityName != "[User]").ToList();
            List<ColumnNameForeignKeyViewModel> checkForeignKey = [];
            foreach (var item in listTable)
            {
                ColumnNameForeignKeyViewModel foreignKeyCreatedBy = new()
                {
                    ColumnName = nameof(BaseEntity.CreatedBy),
                    TableName = item.EntityName,
                };
                checkForeignKey.Add(foreignKeyCreatedBy);
                ColumnNameForeignKeyViewModel foreignKeyModifiedBy = new()
                {
                    ColumnName = nameof(BaseEntity.ModifiedBy),
                    TableName = item.EntityName,
                };
                checkForeignKey.Add(foreignKeyModifiedBy);
                ColumnNameForeignKeyViewModel foreignKeyDeleteddBy = new()
                {
                    ColumnName = nameof(BaseEntity.DeleteddBy),
                    TableName = item.EntityName,
                };
                checkForeignKey.Add(foreignKeyDeleteddBy);
            }
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
            var delete = _unitOfWork.UserRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.UserRepository.Update(delete);
            await _unitOfWork.UserRepository.CommitAsync(typeSave);
            await _userClaimService.SaveAfterUser(id ?? Guid.NewGuid(), string.Empty);
            return string.Empty;
        }

        public async Task<IResult<UserModel>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.UserRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<UserModel>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = new(),
                    };
                }
                var listUserGroupId = string.Join(ConstantHelper.Comma, _userClaimService.GetAllData().ListData.Where(x => x.UserId == data.Id).Select(x => x.UserGroupId.ToString()).ToList());
                UserModel userModel = new()
                {
                    Id = data.Id,
                    UserName = data.UserName,
                    FullName = (await _unitOfWork.PersonRepository.GetSingleAsync(x => x.Id == data.PersonId))?.FullName,
                    PersonId = data.PersonId,
                    IsActive = data.IsActive,
                    IsAdmin = data.IsAdmin,
                    IsDefault = data.IsDefault,
                    ListUserGroupId = !string.IsNullOrEmpty(listUserGroupId) ? listUserGroupId : null,
                };
                return new Result<UserModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = userModel,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetById: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<bool>> ResetPassword(Guid id)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                if (string.IsNullOrEmpty(id.ToString()))
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = false
                    };
                }
                var update = _unitOfWork.UserRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
                if (update == null)
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = false
                    };
                }
                update.PassWord = EncriptFunctionsHelper.GeneratePassword(update.UserName ?? string.Empty);
                _unitOfWork.UserRepository.Update(update);
                await _unitOfWork.UserRepository.CommitAsync(ConstantHelper.TypeSave.Update);
                _unitOfWork.CommitTransaction();
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.ResetPassword,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " ResetPassword: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IResult<bool>> SaveIsActive(Guid id)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                if (string.IsNullOrEmpty(id.ToString()))
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = false
                    };
                }
                var update = _unitOfWork.UserRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
                if (update == null)
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = false
                    };
                }
                update.IsActive = !update.IsActive;
                _unitOfWork.UserRepository.Update(update);
                await _unitOfWork.UserRepository.CommitAsync(ConstantHelper.TypeSave.Update);
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
                Log.Error(string.Concat("[", GetType().Name, "]", " SaveIsActive: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IResult<bool>> SaveIsAdmin(Guid id)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                if (string.IsNullOrEmpty(id.ToString()))
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = false
                    };
                }
                var update = _unitOfWork.UserRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
                if (update == null)
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = false
                    };
                }
                update.IsAdmin = !update.IsAdmin;
                _unitOfWork.UserRepository.Update(update);
                await _unitOfWork.UserRepository.CommitAsync(ConstantHelper.TypeSave.Update);
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
                Log.Error(string.Concat("[", GetType().Name, "]", " SaveIsAdmin: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IResult<bool>> ChangePassword(ChangePasswordRequest request)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var message = string.Empty;
                var exist = _unitOfWork.UserRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
                if (exist == null)
                {
                    message += "Tài khoản của bạn không tồn tại trong hệ thống";
                }
                else
                {
                    if (exist.IsActive == false)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += ", ";
                        }
                        message += "Tài khoản của bạn đang bị khóa, vui lòng liên hệ với quản trị viên";
                    }
                    if (exist.PassWord != EncriptFunctionsHelper.GeneratePassword(string.Concat(request.PassWordOld ?? string.Empty, exist.UserName)))
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += ", ";
                        }
                        message += "Mật khẩu cũ không đúng";
                    }
                    if (request.PassWordNew?.Length < 9)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += ", ";
                        }
                        message += "Mật khẩu mới từ 8 ký tự trở lên";
                    }
                }
                if (!string.IsNullOrEmpty(message))
                {
                    _unitOfWork.CommitTransaction();
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = message,
                        Data = false
                    };
                }
                if (exist != null)
                {
                    exist.PassWord = EncriptFunctionsHelper.GeneratePassword(string.Concat(request.PassWordNew ?? string.Empty, exist.UserName));
                    _unitOfWork.UserRepository.Update(exist);
                    await _unitOfWork.UserRepository.CommitAsync(ConstantHelper.TypeSave.Update);
                }
                _unitOfWork.CommitTransaction();
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = "Thay đổi mật khẩu thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " ChangePassword: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<bool> InsertUserAfter(Guid personId, string? fullName, string typeSave)
        {
            try
            {
                var userName = string.Empty;
                var listUserName = fullName?.Split(ConstantHelper.Space);
                if (listUserName != null && listUserName.Length != 0)
                {
                    var firstName = CommonHelper.RemoveSign4VietnameseString(listUserName[^1]);
                    if (listUserName.Length == 1)
                    {
                        userName = string.Concat(firstName);
                    }
                    if (listUserName.Length == 2)
                    {
                        userName = string.Concat(firstName, listUserName[^2][0]);
                    }
                    if (listUserName.Length == 3)
                    {
                        userName = string.Concat(firstName, listUserName[^3][0], listUserName[^2][0]);
                    }
                    if (listUserName.Length == 4)
                    {
                        userName = string.Concat(firstName, listUserName[^4][0], listUserName[^3][0], listUserName[^2][0]);
                    }
                    if (listUserName.Length == 5)
                    {
                        userName = string.Concat(firstName, listUserName[^5][0], listUserName[^4][0], listUserName[^3][0], listUserName[^2][0]);
                    }
                    if (listUserName.Length == 6)
                    {
                        userName = string.Concat(firstName, listUserName[^6][0], listUserName[^5][0], listUserName[^4][0], listUserName[^3][0], listUserName[^2][0]);
                    }
                }
                var listUserNameDuplicate = _unitOfWork.UserRepository.GetAll().Where(x => (x.UserName ?? string.Empty).Contains(userName)).OrderByDescending(x => x.CreatedDate).ToList();
                if (listUserNameDuplicate.Count != 0)
                {
                    var accountNumber = CommonHelper.FindAndExtractNumber(listUserNameDuplicate[0].UserName ?? string.Empty);
                    userName = string.Concat(userName, accountNumber + 1);
                }
                UserRequest request = new()
                {
                    UserName = userName.ToLower(),
                    PersonId = personId,
                };
                var insert = _mapper.Map<User>(request);
                insert.PassWord = EncriptFunctionsHelper.GeneratePassword(request.UserName ?? string.Empty);
                _unitOfWork.UserRepository.Add(insert);
                await _unitOfWork.UserRepository.CommitAsync(typeSave);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " InsertUserAfter: ", ex.ToString()));
                throw;
            }
        }
    }
}
