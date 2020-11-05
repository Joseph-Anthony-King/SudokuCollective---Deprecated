using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Api.Controllers
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

        // POST: api/games/5
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
                    return Ok(result.Game);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/games
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(
            [FromBody] GetGamesRequest request,
            [FromQuery] bool fullRecord = false)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.GetGames(request, fullRecord);

                if (result.Success)
                {
                    return Ok(result.Games);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(
            int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.DeleteGame(id);

                if (result.Success)
                {
                    return Ok();
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/games/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(
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
                    return BadRequest("Id is incorrect");
                }

                var result =
                    await gamesService.UpdateGame(id, request);

                if (result.Success)
                {
                    return Ok();
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/games
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost, Route("Create")]
        public async Task<ActionResult<Game>> PostGame(
            [FromBody] CreateGameRequest request,
            [FromQuery] bool fullRecord = false)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.CreateGame(request, fullRecord);

                if (result.Success)
                {
                    return CreatedAtAction(
                        "GetUser",
                        "Users",
                        new { id = result.Game.Id },
                        result.Game);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/games/5/checkgame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("{id}/CheckGame")]
        public async Task<ActionResult<Game>> CheckGame(
            int id,
            [FromBody] UpdateGameRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.CheckGame(id, request);

                if (result.Success)
                {
                    return Ok(result.Game);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/games/5/getmygame
        [Authorize(Roles = "USER")]
        [HttpPost, Route("{id}/GetMyGame")]
        public async Task<ActionResult<Game>> GetMyGame(
            int id,
            [FromBody] GetGamesRequest request,
            [FromQuery] bool fullRecord = false)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService.GetMyGame(
                    id,
                    request,
                    fullRecord);

                if (result.Success)
                {
                    return Ok(result.Game);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/games/getmygames
        [Authorize(Roles = "USER")]
        [HttpPost, Route("GetMyGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(
            [FromBody] GetGamesRequest request,
            [FromQuery] bool fullRecord = false)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await gamesService
                    .GetMyGames(request, fullRecord);

                if (result.Success)
                {
                    return Ok(result.Games);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/games/5/deletemygame
        [Authorize(Roles = "USER")]
        [HttpDelete("{id}/DeleteMyGame")]
        public async Task<ActionResult<Game>> DeleteMyGame(
            int id,
            [FromBody] GetGamesRequest request)
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
                    return Ok();
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Request on this License");
            }
        }
    }
}
