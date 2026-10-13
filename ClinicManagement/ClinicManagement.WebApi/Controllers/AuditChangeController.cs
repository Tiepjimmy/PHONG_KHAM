using Microsoft.AspNetCore.Mvc;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.WebApi.Controllers
{
    public class AuditChangeController(IAuditChangeService service) : BaseController
    {
        private readonly IAuditChangeService _service = service;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] AuditChangeSearchRequest request)
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

        [HttpGet("GetAllEntityName")]
        public async Task<IActionResult> GetAllEntityName()
        {
            try
            {
                return Ok(await _service.GetAllEntityName());
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllEntityName: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }

        [HttpGet("GetAllAction")]
        public IActionResult GetAllAction()
        {
            try
            {
                return Ok(_service.GetAllAction());
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllAction: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }

        [HttpGet("DeleteData")]
        public async Task<IActionResult> DeleteData()
        {
            try
            {
                return Ok(await _service.DeleteData());
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " DeleteData: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }
    }
}
