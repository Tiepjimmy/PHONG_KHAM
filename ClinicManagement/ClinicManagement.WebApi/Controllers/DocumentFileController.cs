using Microsoft.AspNetCore.Mvc;
using ClinicManagement.Model.Helper;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.WebApi.Controllers
{
    public class DocumentFileController(IDocumentFileService service) : BaseController
    {
        private readonly IDocumentFileService _service = service;

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                return Ok(await _service.Delete(id));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Delete: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }
    }
}
