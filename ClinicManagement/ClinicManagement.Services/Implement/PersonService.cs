using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request.Custom;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Model.ViewModels.Custom;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class PersonService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IImportService importService, IOptions<AppSettingHelper> options, IAuditChangeService auditChangeService
        , IUserService userService, Data.Share.IAuthenticatedService authenticatedService) : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IImportService _importService = importService;
        private readonly AppSettingHelper _appSettingHelper = options.Value;
        private readonly IUserService _userService = userService;
        private readonly Data.Share.IAuthenticatedService _authenticatedService = authenticatedService;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;

        public async Task<IResults<PersonViewModel>> GetAll(PersonSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@SearchText", request.SearchText);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<PersonViewModel>("uspPerson_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<PersonViewModel>
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

        public IResults<Person> GetAllData()
        {
            try
            {
                var list = _unitOfWork.PersonRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<Person>
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

        public async Task<IResult<bool>> Save(PersonCustomRequest request, string typeSave)
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

        private async Task<string> Insert(PersonCustomRequest request, string typeSave)
        {
            var insert = _mapper.Map<Person>(request);
            _unitOfWork.PersonRepository.Add(insert);
            await _unitOfWork.PersonRepository.CommitAsync(typeSave);
            await _userService.InsertUserAfter(insert.Id, insert.FullName, typeSave);
            return string.Empty;
        }

        private async Task<string> Update(PersonCustomRequest request, string typeSave)
        {
            var exist = _unitOfWork.PersonRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.PersonRepository.Update(update);
            await _unitOfWork.PersonRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkForeignKey = (await _auditChangeService.GetColumnNameForeignKey()).ListData.Where(x => (x.ColumnName ?? string.Empty).Contains(nameof(Person))).ToList();
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
            var delete = _unitOfWork.PersonRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.PersonRepository.Update(delete);
            await _unitOfWork.PersonRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        public async Task<IResult<Person>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.PersonRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<Person>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                data.Avatar = !string.IsNullOrEmpty(data.Avatar) ? Path.Combine(_appSettingHelper.DomainFile ?? string.Empty, ConstantHelper.Import, ConstantHelper.FolderName.PERSON_IMAGE, data.Avatar) : string.Empty;
                return new Result<Person>
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

        public async Task<IResults<PersonViewModel>> GetAllDataByKey(string key, bool isEdit)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@Key", key);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<PersonViewModel>("uspPerson_GetAllDataByKey", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    if (isEdit)
                    {
                        list = [.. list.Take(10).OrderBy(x => x.FullName)];
                    }
                    else
                    {
                        var listPersonId = _unitOfWork.UserRepository.GetAll().Where(x => x.IsActive && !x.IsDeleted).Select(x => x.PersonId).ToList();
                        list = [.. list.Where(x => !listPersonId.Contains(x.Id)).Take(10).OrderBy(x => x.FullName)];
                    }
                }
                return new Results<PersonViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByKey: ", ex.ToString()));
                throw;
            }
        }

        public IResult<PersonCustomViewModel> GetProfile()
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@Id", _authenticatedService.Id);
                var data = _dapperRepository.QueryFirstOrDefaultUsingStoreProc<PersonCustomViewModel>("uspPerson_GetProfile", dynamicParameters);
                data.Avatar = !string.IsNullOrEmpty(data.Avatar) ? Path.Combine(_appSettingHelper.DomainFile ?? string.Empty, ConstantHelper.Import, ConstantHelper.FolderName.PERSON_IMAGE, data.Avatar) : string.Empty;
                return new Result<PersonCustomViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = data,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetProfile: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<bool>> UpdateProfile(PersonCustomRequest request)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var update = _unitOfWork.PersonRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
                if (string.IsNullOrEmpty(request.Id.ToString()) || update == null)
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = false
                    };
                }
                if (request.File != null)
                {
                    List<IFormFile> listFile =
                    [
                        request.File
                    ];
                    var uploadFileResponse = _importService.UploadFileForInsertUpdate(listFile, ConstantHelper.FolderName.PERSON_IMAGE);
                    if (uploadFileResponse.Result)
                    {
                        if (uploadFileResponse.ListFile != null)
                        {
                            update.Avatar = uploadFileResponse.ListFile?.FirstOrDefault()?.FileNameHas ?? string.Empty;
                        }
                    }
                    else
                    {
                        return new Result<bool>
                        {
                            ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                            ResponseMessage = ConstantHelper.Message.UploadFileFail,
                            Data = false
                        };
                    }
                }
                update.FullName = request.FullName;
                update.PhoneNumber = request.PhoneNumber;
                update.DateOfBirth = request.DateOfBirth;
                update.Sex = request.Sex;
                update.Address = request.Address;
                _unitOfWork.PersonRepository.Update(update);
                await _unitOfWork.PersonRepository.CommitAsync(ConstantHelper.TypeSave.UpdateProfile);
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = !string.IsNullOrEmpty(update.Avatar) ? Path.Combine(_appSettingHelper.DomainFile ?? string.Empty, ConstantHelper.Import, ConstantHelper.FolderName.PERSON_IMAGE, update.Avatar) : string.Empty,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " UpdateProfile: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
