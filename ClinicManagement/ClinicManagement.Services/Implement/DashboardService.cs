using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Model;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class DashboardService : IDashboardService
    {

        public DashboardService()
        {
        }

        public IResult<DashboardModel> GetCount()
        {
            try
            {
                DashboardModel data = new();
                return new Result<DashboardModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = data,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetCount: ", ex.ToString()));
                throw;
            }
        }

        public IResult<DashboardModel> GetListChartOptionsBar()
        {
            try
            {
                DashboardModel data = new();
                return new Result<DashboardModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = data,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetListChartOptionsBar: ", ex.ToString()));
                throw;
            }
        }

        public IResult<DashboardModel> GetListChartOptionsPolarArea()
        {
            try
            {
                DashboardModel data = new();
                return new Result<DashboardModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = data,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetListChartOptionsPolarArea: ", ex.ToString()));
                throw;
            }
        }
    }
}
