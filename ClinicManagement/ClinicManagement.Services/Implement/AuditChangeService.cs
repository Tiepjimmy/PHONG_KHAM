using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Model.ViewModels.Custom;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class AuditChangeService(IUnitOfWork unitOfWork, IMapper mapper, IDapperRepository dapperRepository) : IAuditChangeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IDapperRepository _dapperRepository = dapperRepository;

        public async Task<IResults<AuditChangeCustomViewModel>> GetAll(AuditChangeSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@IdChange", request.IdChange?.ToString());
                dynamicParameters.Add("@Action", request.Action);
                dynamicParameters.Add("@EntityName", request.EntityName);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<AuditChangeCustomViewModel>("uspAuditChange_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<AuditChangeCustomViewModel>
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

        public async Task<IResults<AuditChangeViewModel>> GetAllEntityName()
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<AuditChangeViewModel>("uspTableName_GetAll", dynamicParameters)).ToList();
                return new Results<AuditChangeViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllEntityName: ", ex.ToString()));
                throw;
            }
        }

        public IResults<AuditChangeViewModel> GetAllAction()
        {
            try
            {
                List<AuditChangeViewModel> list =
                [
                    new() { Action = nameof(EntityState.Detached) },
                    new() { Action = nameof(EntityState.Unchanged) },
                    new() { Action = nameof(EntityState.Deleted) },
                    new() { Action = nameof(EntityState.Modified) },
                    new() { Action = nameof(EntityState.Added) },
                ];
                return new Results<AuditChangeViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllAction: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<bool>> DeleteData()
        {
            try
            {
                var listTableName = (await GetAllEntityName()).ListData;
                string strSql = string.Empty;
                foreach (var item in listTableName)
                {
                    strSql += string.Concat("DELETE FROM ", item.EntityName, " where IsDeleted = 1");
                }
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@StrSql", strSql);
                _dapperRepository.InsertOrUpdateUsingStoreProc<bool>("uspSystem_ExecuteQuery", dynamicParameters);
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.DeleteSuccess,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " DeleteData: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResults<ColumnNameForeignKeyViewModel>> GetColumnNameForeignKey()
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<ColumnNameForeignKeyViewModel>("uspSystem_GetColumnNameForeignKey", dynamicParameters)).ToList();
                return new Results<ColumnNameForeignKeyViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetColumnNameForeignKey: ", ex.ToString()));
                throw;
            }
        }
    }
}
