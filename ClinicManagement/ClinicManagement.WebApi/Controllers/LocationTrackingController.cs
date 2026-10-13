using Microsoft.AspNetCore.Mvc;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.WebApi.Controllers
{
    public class LocationTrackingController(ILocationTrackingService service) : BaseController
    {
        private readonly ILocationTrackingService _service = service;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] LocationTrackingSearchRequest request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(ConstantHelper.Message.ModelStateMessage);
                }
                return Ok(await _service.GetAll(request));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAll: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }

        [HttpGet("DeleteData")]
        public IActionResult DeleteData()
        {
            try
            {
                return Ok(_service.DeleteData());
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " DeleteData: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }
    }
}
