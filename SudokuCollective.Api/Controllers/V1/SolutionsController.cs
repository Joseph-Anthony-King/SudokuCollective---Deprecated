using System;
using System.Collections.Generic;
using System.Net;
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
    public class SolutionsController : ControllerBase
    {
        private readonly ISolutionsService solutionService;
        private readonly IAppsService appsService;

        public SolutionsController(ISolutionsService solutionServ,
            IAppsService appsServ)
        {
            solutionService = solutionServ;
            appsService = appsServ;
        }

        // POST: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost("{id}")]
        public async Task<ActionResult<SudokuSolution>> Get(
            int id,
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await solutionService.Get(id);

                    if (result.IsSuccess)
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
                        IsSuccess = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    IsSuccess = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // POST: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<SudokuSolution>>> GetSolutions(
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await solutionService
                        .GetSolutions(request);

                    if (result.IsSuccess)
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
                        IsSuccess = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    IsSuccess = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // POST: api/solutions
        [AllowAnonymous]
        [HttpPost, Route("Solve")]
        public async Task<ActionResult<SudokuSolution>> Solve(
            [FromBody] SolutionRequest request)
        {
            try
            {
                var result = await solutionService.Solve(request);

                if (result.IsSuccess)
                {
                    if (result.Solution != null)
                    {
                        result.Message = ControllerMessages.StatusCode200(result.Message);

                        return Ok(result);
                    }
                    else
                    {
                        result.Message = ControllerMessages.StatusCode200(result.Message);

                        return Ok(result);
                    }
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
                    IsSuccess = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // POST: api/solutions/generate
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("Generate")]
        public async Task<ActionResult<SudokuSolution>> Generate()
        {
            try
            {
                var result = await solutionService.Generate();

                if (result.IsSuccess)
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
                    IsSuccess = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        // POST: api/solutions/addsolutions
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost, Route("AddSolutions")]
        public async Task<IActionResult> AddSolutions(
            [FromBody] AddSolutionRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    if (request.Limit <= 1000)
                    {
                        var result = await solutionService.Add(request.Limit);

                        if (result.IsSuccess)
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
                        return BadRequest(
                            ControllerMessages.StatusCode400(
                                string.Format(
                                    "The Amount Of Solutions Requested, {0}, Exceeds The Service's 1,000 Limit",
                                    request.Limit.ToString())
                                ));
                    }
                }
                else
                {
                    var result = new BaseResult
                    {
                        IsSuccess = false,
                        Message = ControllerMessages.InvalidLicenseRequestMessage
                    };

                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    IsSuccess = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }
    }
}
