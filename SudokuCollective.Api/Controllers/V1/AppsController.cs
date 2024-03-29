using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Enums;
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
    public class AppsController : ControllerBase
    {
        private readonly IAppsService appsService;

        public AppsController(IAppsService appsServ)
        {
            appsService = appsServ;
        }

        // POST: api/apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("{id}")]
        public async Task<ActionResult<App>> Get(
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
                    var result = await appsService.Get(id);

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

        // PUT: api/apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] AppRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService.Update(id, request);

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

        // DELETE: api/apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> Delete(
            int id,
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsOwnerOfThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService.DeleteOrReset(id);

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
                        Message = ControllerMessages.NotOwnerMessage
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

        // POST: api/apps/getByLicense
        [AllowAnonymous]
        [HttpPost, Route("GetByLicense")]
        public async Task<ActionResult<App>> GetByLicense(
            [FromBody] BaseRequest request)
        {
            try
            {
                var result = await appsService
                    .GetAppByLicense(request.License, request.RequestorId);

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

        // POST: api/apps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<App>>> GetApps(
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService
                        .GetApps(request.Paginator, request.RequestorId);

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

        // POST: api/apps/GetMyApps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("GetMyApps")]
        public async Task<ActionResult<IEnumerable<App>>> GetMyApps(
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService
                        .GetMyApps(
                        request.RequestorId,
                        request.Paginator);

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

        // POST: api/apps/getMyRegistered
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost, Route("GetMyRegistered/{userId}")]
        public async Task<ActionResult> RegisteredApps(
            int userId,
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService.GetRegisteredApps(
                        userId,
                        request.Paginator);

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

        // POST: api/apps/5/getAppUsers
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("{id}/GetAppUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAppUsers(
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
                    var result = await appsService
                        .GetAppUsers(
                            id,
                            request.RequestorId,
                            request.Paginator,
                            true);

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

        // POST: api/apps/5/getNonAppUsers
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("{id}/GetNonAppUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetNonAppUsers(
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
                    var result = await appsService
                        .GetAppUsers(
                            id,
                            request.RequestorId,
                            request.Paginator,
                            false);

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

        // PUT: api/apps/5/adduser/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/AddUser/{userId}")]
        public async Task<IActionResult> AddUser(
            int id,
            int userId,
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService.AddAppUser(id, userId);

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

        // DELETE: api/apps/5/removeuser/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}/RemoveUser/{userId}")]
        public async Task<IActionResult> RemoveUser(
            int id,
            int userId, 
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService.RemoveAppUser(id, userId);

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

        // PUT: api/apps/5/activate
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/Activate")]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                var result = await appsService.Activate(id);

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

        // PUT: api/apps/5/deactivate
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/Deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var result = await appsService.Deactivate(id);

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

        // PUT: api/apps/5/reset
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/Reset")]
        public async Task<ActionResult> Reset(
            int id,
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsOwnerOfThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService.DeleteOrReset(id, true);

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
                        Message = ControllerMessages.NotOwnerMessage
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

        // PUT: api/apps/5/activateAdminPrivileges/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("{id}/ActivateAdminPrivileges/{userId}")]
        public async Task<ActionResult> ActivateAdminPrivileges(
            int id,
            int userId, 
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService.ActivateAdminPrivileges(id, userId);

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

        // PUT: api/apps/deactivateAdminPrivileges/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/DeactivateAdminPrivileges/{userId}")]
        public async Task<ActionResult> DeactivateAdminPrivileges(
            int id,
            int userId,
            [FromBody] BaseRequest request)
        {
            try
            {
                if (await appsService.IsRequestValidOnThisLicense(
                    request.AppId,
                    request.License,
                    request.RequestorId))
                {
                    var result = await appsService.DeactivateAdminPrivileges(id, userId);

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

        // GET: api/apps/getTimeFrames
        [AllowAnonymous]
        [HttpGet, Route("timeFrames")]
        public ActionResult<List<EnumListItem>> GetTimeFrames()
        {
            var authToken = new List<string> { "auth token access period" };

            var result = new List<EnumListItem>
            {
                new EnumListItem { 
                    Label = "Seconds", 
                    Value = (int)TimeFrame.SECONDS,
                    AppliesTo = authToken },
                new EnumListItem { 
                    Label = "Minutes", 
                    Value = (int)TimeFrame.MINUTES,
                    AppliesTo = authToken },
                new EnumListItem { 
                    Label = "Hours", 
                    Value = (int)TimeFrame.HOURS,
                    AppliesTo = authToken },
                new EnumListItem { 
                    Label = "Days", 
                    Value = (int)TimeFrame.DAYS,
                    AppliesTo = authToken },
                new EnumListItem { 
                    Label = "Months", 
                    Value = (int)TimeFrame.MONTHS,
                    AppliesTo = authToken },
            };

            return Ok(result);
        }

        // GET: api/apps/sortValues
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet, Route("sortValues")]
        public ActionResult<List<EnumListItem>> sortValues()
        {
            var all = new List<string> { "app", "game", "user" };
            var app = new List<string> { "app" };
            var game = new List<string> { "game" };
            var user = new List<string> { "user" };
            
            var result = new List<EnumListItem>
            {
                new EnumListItem { 
                    Label = "Null", 
                    Value = (int)SortValue.NULL,
                    AppliesTo = all },
                new EnumListItem { 
                    Label = "ID", 
                    Value = (int)SortValue.ID,
                    AppliesTo = all },
                new EnumListItem { 
                    Label = "Username", 
                    Value = (int)SortValue.USERNAME,
                    AppliesTo = user },
                new EnumListItem { 
                    Label = "First Name", 
                    Value = (int)SortValue.FIRSTNAME,
                    AppliesTo = user },
                new EnumListItem { 
                    Label = "Last Name", 
                    Value = (int)SortValue.LASTNAME,
                    AppliesTo = user },
                new EnumListItem { 
                    Label = "Full Name", 
                    Value = (int)SortValue.FULLNAME,
                    AppliesTo = user },
                new EnumListItem { 
                    Label = "Nick Name", 
                    Value = (int)SortValue.NICKNAME,
                    AppliesTo = user },
                new EnumListItem { 
                    Label = "Game Count", 
                    Value = (int)SortValue.GAMECOUNT,
                    AppliesTo = user },
                new EnumListItem { 
                    Label = "App Count", 
                    Value = (int)SortValue.APPCOUNT,
                    AppliesTo = user },
                new EnumListItem { 
                    Label = "Name", 
                    Value = (int)SortValue.NAME,
                    AppliesTo = app },
                new EnumListItem { 
                    Label = "Date Created", 
                    Value = (int)SortValue.DATECREATED,
                    AppliesTo = all },
                new EnumListItem { 
                    Label = "Date Updated", 
                    Value = (int)SortValue.DATEUPDATED,
                    AppliesTo = all },
                new EnumListItem { 
                    Label = "Difficulty Level", 
                    Value = (int)SortValue.DIFFICULTYLEVEL,
                    AppliesTo = game },
                new EnumListItem {
                    Label = "User Count",
                    Value = (int)SortValue.USERCOUNT,
                    AppliesTo = app },
                new EnumListItem { 
                    Label = "Score", 
                    Value = (int)SortValue.SCORE,
                    AppliesTo = game }
            };

            return Ok(result);
        }

        public class EnumListItem 
        {
            public string Label;
            public int Value;
            public List<string> AppliesTo;
        };
    }
}
