using Dapper;
using Newtonsoft.Json;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class UserRoleService(IUnitOfWork unitOfWork, IObjectMenuService objectMenuService, IDapperRepository dapperRepository) : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IObjectMenuService _objectMenuService = objectMenuService;

        public async Task<IResult<bool>> DeleteByUserGroupId(Guid userGroupId)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var list = GetAllData().ListData.Where(x => x.UserGroupId == userGroupId).ToList();
                if (list.Count != 0)
                {
                    foreach (var item in list)
                    {
                        _unitOfWork.UserRoleRepository.Update(item);
                    }
                    await _unitOfWork.UserRoleRepository.CommitAsync(ConstantHelper.TypeSave.Delete);
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
                Log.Error(string.Concat("[", GetType().Name, "]", " DeleteByUserGroupId: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public IResults<UserRole> GetAllData()
        {
            try
            {
                var list = _unitOfWork.UserRoleRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<UserRole>
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

        public async Task<IResult<UserRoleModel>> GetByUserGroupId(string listUserGroupId, bool isUseManage)
        {
            try
            {
                List<UserRoleViewModel> listMenu = [];
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@ListUserGroupId", listUserGroupId);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<UserRoleViewModel>("uspUserRole_GetByUserGroupId", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    List<UserRoleViewModel> listLevel1 = [.. list.Where(x => x.LevelShow == 1).OrderBy(x => x.OrderShow)];
                    if (!isUseManage)
                    {
                        listLevel1 = listLevel1.Where(x => x.AllowAccess).DistinctBy(x => x.ObjectMenuName).ToList();
                    }
                    List<UserRoleViewModel> listLevel2 = [.. list.Where(x => x.LevelShow == 2).OrderBy(x => x.OrderShow)];
                    if (!isUseManage)
                    {
                        listLevel2 = listLevel2.Where(x => x.AllowAccess).DistinctBy(x => x.ObjectMenuName).ToList();
                    }
                    foreach (var item1 in listLevel1)
                    {
                        List<UserRoleViewModel> listMenuLevel2 = [];
                        foreach (var item2 in listLevel2.Where(x => x.ParentId == item1.ObjectMenuId))
                        {
                            UserRoleViewModel menuLevel2 = new()
                            {
                                Id = item2.Id,
                                ObjectMenuName = item2.ObjectMenuName,
                                ShortName = item2.ShortName,
                                ParentId = item1.Id,
                                MetaLink = item2.MetaLink,
                                IconName = item2.IconName,
                                LevelShow = item2.LevelShow,
                                OrderShow = item2.OrderShow,
                                DataTarget = item2.DataTarget,
                                AllowAccess = item2.AllowAccess,
                                AllowAdd = item2.AllowAdd,
                                AllowEdit = item2.AllowEdit,
                                AllowDelete = item2.AllowDelete,
                                AllowPrint = item2.AllowPrint,
                                AllowExport = item2.AllowExport,
                                AllowImport = item2.AllowImport,
                                Children = []
                            };
                            listMenuLevel2.Add(menuLevel2);
                        }
                        UserRoleViewModel menuLevel1 = new()
                        {
                            Id = item1.Id,
                            ObjectMenuName = item1.ObjectMenuName,
                            ShortName = item1.ShortName,
                            MetaLink = item1.MetaLink,
                            IconName = item1.IconName,
                            LevelShow = item1.LevelShow,
                            OrderShow = item1.OrderShow,
                            DataTarget = item1.DataTarget,
                            AriaControls = item1.AriaControls,
                            AllowAccess = item1.AllowAccess,
                            AllowAdd = item1.AllowAdd,
                            AllowEdit = item1.AllowEdit,
                            AllowDelete = item1.AllowDelete,
                            AllowPrint = item1.AllowPrint,
                            AllowExport = item1.AllowExport,
                            AllowImport = item1.AllowImport,
                            Children = listMenuLevel2
                        };
                        listMenu.Add(menuLevel1);
                    }
                }
                UserRoleModel userRoleModel = new()
                {
                    ListMenu = listMenu,
                };
                return new Result<UserRoleModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = userRoleModel
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetByUserGroupId: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<bool>> Insert(Guid userGroupId)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var listObjectMenu = _objectMenuService.GetAllData().ListData;
                foreach (var item in listObjectMenu)
                {
                    UserRole model = new()
                    {
                        Id = Guid.NewGuid(),
                        UserGroupId = userGroupId,
                        ObjectMenuId = item.Id,
                        AllowAccess = true,
                        AllowAdd = true,
                        AllowEdit = true,
                        AllowDelete = true,
                        AllowPrint = true,
                        AllowExport = true,
                        AllowImport = true,
                    };
                    _unitOfWork.UserRoleRepository.Add(model);
                }
                await _unitOfWork.UserRoleRepository.CommitAsync(ConstantHelper.TypeSave.Insert);
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
                Log.Error(string.Concat("[", GetType().Name, "]", " Insert: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IResult<bool>> Update(UserRoleRequest request)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var result = string.Empty;
                var lstRole = JsonConvert.DeserializeObject<List<UserRole>>(request.ListPermission ?? string.Empty);
                if (lstRole?.Count > 0)
                {
                    List<UserRole> listUpdate = [];
                    var listId = string.Join(ConstantHelper.Comma, lstRole.Select(x => x.Id)).Split(ConstantHelper.Comma).ToList();
                    var list = GetAllData().ListData.Where(x => listId.Contains(x.Id.ToString() ?? string.Empty)).ToList();
                    foreach (var item in lstRole)
                    {
                        var getItem = list.FirstOrDefault(x => x.Id == item.Id);
                        if (getItem != null)
                        {
                            getItem.UserGroupId = getItem.UserGroupId;
                            getItem.ObjectMenuId = getItem.ObjectMenuId;
                            getItem.AllowAccess = item.AllowAccess;
                            getItem.AllowAdd = item.AllowAdd;
                            getItem.AllowEdit = item.AllowEdit;
                            getItem.AllowDelete = item.AllowDelete;
                            getItem.AllowPrint = item.AllowPrint;
                            getItem.AllowExport = item.AllowExport;
                            getItem.AllowImport = item.AllowImport;
                            _unitOfWork.UserRoleRepository.Update(getItem);
                        }
                    }
                    await _unitOfWork.UserRoleRepository.CommitAsync(ConstantHelper.TypeSave.Update);
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
                    ResponseMessage = ConstantHelper.Message.SaveSuccess,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Update: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
