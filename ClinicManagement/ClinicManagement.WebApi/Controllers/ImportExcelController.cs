using Microsoft.AspNetCore.Mvc;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.WebApi.Controllers
{
    public class ImportExcelController(IImportService service) : BaseController
    {
        private readonly IImportService _service = service;

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromForm] ImportExcelRequest request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(ConstantHelper.Message.ModelStateMessage);
                }
                return Ok(await _service.Save(request));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Save: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }
    }
}
