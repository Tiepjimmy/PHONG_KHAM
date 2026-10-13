using Dapper;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels.Custom;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class LocationTrackingService(IDapperRepository dapperRepository) : ILocationTrackingService
    {
        private readonly IDapperRepository _dapperRepository = dapperRepository;

        public IResult<bool> DeleteData()
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                var list = _dapperRepository.InsertOrUpdateUsingStoreProc<bool>("uspLocationTracking_DeleteData", dynamicParameters);
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

        public async Task<IResults<LocationTrackingCustomViewModel>> GetAll(LocationTrackingSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@SearchText", request.SearchText);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<LocationTrackingCustomViewModel>("uspLocationTracking_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<LocationTrackingCustomViewModel>
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
    }
}
