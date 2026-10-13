using Autofac.Core;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace ClinicManagement.WebApi.Controllers
{
    public class BranchController(IBranchService service) : BaseController
    {
        private readonly IBranchService _service = service;
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] BranchSearchRequest request)
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

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromForm] BranchRequest request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(ConstantHelper.Message.ModelStateMessage);
                }
                return Ok(await _service.Save(request, ConstantHelper.TypeSave.Insert));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Insert: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] BranchRequest request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(ConstantHelper.Message.ModelStateMessage);
                }
                return Ok(await _service.Save(request, ConstantHelper.TypeSave.Update));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Update: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                BranchRequest request = new()
                {
                    Id = id
                };
                return Ok(await _service.Save(request, ConstantHelper.TypeSave.Delete));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Delete: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                return Ok(await _service.GetById(id));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetById: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }

        [HttpGet("GetAllData")]
        public IActionResult GetAllData()
        {
            try
            {
                return Ok(_service.GetAllData());
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllData: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }
        [HttpGet("GetAllDataByRegionId")]
        public IActionResult GetAllDataByRegionId(Guid? regionId)
        {
            try
            {
                return Ok(_service.GetAllDataByRegionId(regionId));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByRegionId: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }
        [HttpGet("GetAllDataByAreaId")]
        public IActionResult GetAllDataByAreaId(Guid? areaId)
        {
            try
            {
                return Ok(_service.GetAllDataByAreaId(areaId));
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllDataByAreaId: ", ex.ToString()));
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), ConstantHelper.Message.InternalServer);
            }
        }
    }
}
