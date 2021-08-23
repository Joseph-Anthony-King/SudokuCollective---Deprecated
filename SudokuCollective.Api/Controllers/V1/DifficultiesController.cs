using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Api.V1.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DifficultiesController : ControllerBase
    {
        private readonly IDifficultiesService difficultiesService;
        private readonly IAppsService appsService;

        public DifficultiesController(
            IDifficultiesService difficultiesServ,
            IAppsService appsServ)
        {
            difficultiesService = difficultiesServ;
            appsService = appsServ;
        }

        // POST: api/v1/difficulties
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost]
        public async Task<ActionResult<Difficulty>> Post(
            [FromBody] CreateDifficultyRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await difficultiesService
                        .Create(request.Name, request.DisplayName, request.DifficultyLevel);

                    if (result.Success)
                    {
                        result.Message = ControllerMessages.StatusCode201(result.Message);

                        return StatusCode((int)HttpStatusCode.Created, result);
                    }
                    else
                    {
                        result.Message = ControllerMessages.StatusCode404(result.Message);

                        return NotFound(result);
                    }
                }
                else
                {
                    var result = new BaseResult
                    {
                        Success = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // PUT: api/v1/difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,
            [FromBody] UpdateDifficultyRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    if (id != request.Id)
                    {
                        return BadRequest(ControllerMessages.IdIncorrectMessage);
                    }

                    var result = await difficultiesService.Update(id, request);

                    if (result.Success)
                    {
                        result.Message = ControllerMessages.StatusCode200(result.Message);

                        return Ok(result);
                    }
                    else
                    {
                        result.Message = ControllerMessages.StatusCode404(result.Message);

                        return NotFound(result);
                    }
                }
                else
                {
                    var result = new BaseResult
                    {
                        Success = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // DELETE: api/v1/difficulties/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id,
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await difficultiesService.Delete(id);

                    if (result.Success)
                    {
                        result.Message = ControllerMessages.StatusCode200(result.Message);

                        return Ok(result);
                    }
                    else
                    {
                        result.Message = ControllerMessages.StatusCode404(result.Message);

                        return NotFound(result.Message);
                    }
                }
                else
                {
                    var result = new BaseResult
                    {
                        Success = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            } 
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // GET: api/v1/difficulties/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Difficulty>> Get(int id)
        {
            try
            {
                var result = await difficultiesService.Get(id);

                if (result.Success)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // GET: api/v1/difficulties
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties()
        {
            try
            {
                var result = await difficultiesService.GetDifficulties();

                if (result.Success)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);

                    return NotFound(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    Success = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }
    }
}
