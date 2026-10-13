using Microsoft.AspNetCore.Mvc;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.WebApi.Controllers
{
    public class UserRoleController(IUserRoleService service) : BaseController
    {
        private readonly IUserRoleService _service = service;

        [HttpGet("GetByUserGroupId")]
        public async Task<IActionResult> GetByUserGroupId(Guid userGroupId)
        {
            try
            {
                return Ok(await _service.GetByUserGroupId(userGroupId.ToString(), true));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetByUserGroupId: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] UserRoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(ConstantHelper.Message.ModelStateMessage);
                }
                return Ok(await _service.Update(request));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Update: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }
    }
}
