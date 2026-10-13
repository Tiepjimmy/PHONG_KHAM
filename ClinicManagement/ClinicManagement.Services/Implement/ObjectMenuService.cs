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
    public class ObjectMenuService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper) : IObjectMenuService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IResults<ObjectMenuViewModel>> GetAll(ObjectMenuSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@SearchText", request.SearchText);
                dynamicParameters.Add("@ParentId", request.ParentId);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<ObjectMenuViewModel>("uspObjectMenu_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<ObjectMenuViewModel>
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

        public IResults<ObjectMenu> GetAllData()
        {
            try
            {
                var list = _unitOfWork.ObjectMenuRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<ObjectMenu>
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

        public async Task<IResult<ObjectMenu>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.ObjectMenuRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<ObjectMenu>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = data,
                    };
                }
                return new Result<ObjectMenu>
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

        public async Task<IResult<bool>> Save(ObjectMenuRequest request, string typeSave)
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

        private async Task<string> Insert(ObjectMenuRequest request, string typeSave)
        {
            var insert = _mapper.Map<ObjectMenu>(request);
            _unitOfWork.ObjectMenuRepository.Add(insert);
            await _unitOfWork.ObjectMenuRepository.CommitAsync(typeSave);
            var listUserGroup = _unitOfWork.UserGroupRepository.GetAll().Where(x => !x.IsDeleted).ToList();
            if (listUserGroup.Count != 0)
            {
                foreach (var item in listUserGroup)
                {
                    UserRole userRole = new()
                    {
                        Id = Guid.NewGuid(),
                        UserGroupId = item.Id,
                        ObjectMenuId = insert.Id,
                    };
                    _unitOfWork.UserRoleRepository.Add(userRole);
                }
                await _unitOfWork.UserRoleRepository.CommitAsync(typeSave);
            }
            return string.Empty;
        }

        private async Task<string> Update(ObjectMenuRequest request, string typeSave)
        {
            var exist = _unitOfWork.ObjectMenuRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.ObjectMenuRepository.Update(update);
            await _unitOfWork.ObjectMenuRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var delete = _unitOfWork.ObjectMenuRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.ObjectMenuRepository.Update(delete);
            await _unitOfWork.ObjectMenuRepository.CommitAsync(typeSave);
            var list = _unitOfWork.UserRoleRepository.GetAll().Where(x => x.ObjectMenuId == id && !x.IsDeleted).ToList();
            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    _unitOfWork.UserRoleRepository.Update(item);
                }
                await _unitOfWork.UserRoleRepository.CommitAsync(ConstantHelper.TypeSave.Delete);
            }
            return string.Empty;
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
                var update = _unitOfWork.ObjectMenuRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
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
                _unitOfWork.ObjectMenuRepository.Update(update);
                await _unitOfWork.ObjectMenuRepository.CommitAsync(ConstantHelper.TypeSave.Update);
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
    }
}
