using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.RequestModels.GameRequests;
using SudokuCollective.Core.Enums;

namespace SudokuCollective.Api.V1.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService gamesService;
        private readonly IAppsService appsService;

        public GamesController(
            IGamesService gamesServ,
            IAppsService appsServ)
        {
            gamesService = gamesServ;
            appsService = appsServ;
        }

        // POST: api/games
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<Game>> Post(
            [FromBody] CreateGameRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.Create(request);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT:  api/v1/games/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateGameRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                if (id != request.GameId)
                {
                    return BadRequest(ControllerMessages.IdIncorrectMessage);
                }

                var result =
                    await gamesService.Update(id, request);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // DELETE:  api/v1/games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> Delete(
            int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.Delete(id);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // POST:  api/v1/games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.GetGame(
                    id, request.AppId);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // POST:  api/v1/games
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("GetGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(
            [FromBody] GamesRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.GetGames(request);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // POST:  api/v1/games/5/getmygame
        [Authorize(Roles = "USER")]
        [HttpPost, Route("{id}/GetMyGame")]
        public async Task<ActionResult<Game>> GetMyGame(
            int id,
            [FromBody] GamesRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.GetMyGame(
                    id,
                    request);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // POST:  api/v1/games/getmygames
        [Authorize(Roles = "USER")]
        [HttpPost, Route("GetMyGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(
            [FromBody] GamesRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService
                    .GetMyGames(request);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // DELETE:  api/v1/games/5/deletemygame
        [Authorize(Roles = "USER")]
        [HttpDelete("{id}/DeleteMyGame")]
        public async Task<ActionResult<Game>> DeleteMyGame(
            int id,
            [FromBody] GamesRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.DeleteMyGame(
                    id,
                    request);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT:  api/v1/games/5/check
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("{id}/Check")]
        public async Task<ActionResult<Game>> Check(
            int id,
            [FromBody] UpdateGameRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.Check(id, request);

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
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // GET:  api/v1/games/createAnnonymous
        [AllowAnonymous]
        [HttpGet("CreateAnnonymous")]
        public async Task<ActionResult> CreateAnnonymous([FromQuery] AnnonymousGameRequest request)
        {
            if (request.DifficultyLevel == DifficultyLevel.NULL)
            {
                return BadRequest(
                    ControllerMessages.StatusCode400(
                        GamesMessages.DifficultyLevelIsRequiredMessage));
            }

            var result = await gamesService.CreateAnnonymous(request.DifficultyLevel);

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

        // POST:  api/v1/games/checkAnnonymous
        [AllowAnonymous]
        [HttpPost("CheckAnnonymous")]
        public async Task<ActionResult> CheckAnnonymous([FromBody] AnnonymousCheckRequest request)
        {
            var intList = new List<int>();

            intList.AddRange(request.FirstRow);
            intList.AddRange(request.SecondRow);
            intList.AddRange(request.ThirdRow);
            intList.AddRange(request.FourthRow);
            intList.AddRange(request.FifthRow);
            intList.AddRange(request.SixthRow);
            intList.AddRange(request.SeventhRow);
            intList.AddRange(request.EighthRow);
            intList.AddRange(request.NinthRow);

            var result = await gamesService.CheckAnnonymous(intList);

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
    }
}
