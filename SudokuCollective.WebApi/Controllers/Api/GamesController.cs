using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Domain;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase {

        private readonly IGamesService _gamesService;
        private readonly IAppsService _appsService;

        public GamesController(IGamesService gamesService, 
            IAppsService appsService) {
            
            _gamesService = gamesService;
            _appsService = appsService;
        }

        // GET: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id, 
            [FromBody] BaseRequest baseRequest) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _gamesService.GetGame(
                    id, baseRequest.AppId);

                if (result.Success) {
                    
                    return Ok(result.Game);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(
            [FromBody] BaseRequest baseRequest, 
            [FromQuery] bool fullRecord = false) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _gamesService.GetGames(baseRequest, fullRecord);

                if (result.Success) {

                    return Ok(result.Games);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(
            int id,
            [FromBody] BaseRequest baseRequest) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License, 
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _gamesService.DeleteGame(id);

                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Games/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(
            int id, 
            [FromBody] UpdateGameRequest updateGameRequest) {

            if (await _appsService.IsRequestValidOnThisLicense(
                updateGameRequest.License,
                updateGameRequest.RequestorId,
                updateGameRequest.AppId)) {

                if (id != updateGameRequest.GameId) {

                    return BadRequest("Id is incorrect");
                }

                var result = 
                    await _gamesService.UpdateGame(id, updateGameRequest);
                
                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            } 
        }

        // POST: api/Games
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(
            [FromBody] CreateGameRequest createGameRequest,
            [FromQuery] bool fullRecord = false) {

            if (await _appsService.IsRequestValidOnThisLicense(
                createGameRequest.License,
                createGameRequest.RequestorId,
                createGameRequest.AppId)) {
            
                var result = await _gamesService.CreateGame(createGameRequest, fullRecord);

                if (result.Success) {

                    return CreatedAtAction(
                        "GetUser",
                        "Users",
                        new { id = result.Game.Id },
                        result.Game);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Games/5/CheckGame
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut, Route("{id}/CheckGame")]
        public async Task<ActionResult<Game>> CheckGame(
            int id,
            [FromBody] UpdateGameRequest updateGameRequest) {

            if (await _appsService.IsRequestValidOnThisLicense(
                updateGameRequest.License,
                updateGameRequest.RequestorId,
                updateGameRequest.AppId)) {

                var result = await _gamesService.CheckGame(id, updateGameRequest);

                if (result.Success) {
                    
                    return Ok(result.Game);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }
        
        // GET: api/Games/5/GetMyGame
        [Authorize(Roles = "USER")]
        [HttpPost, Route("{id}/GetMyGame")]
        public async Task<ActionResult<Game>> GetMyGame(
            int id,
            [FromBody] GetMyGameRequest getMyGameRequest, 
            [FromQuery] bool fullRecord = false) {

            if (await _appsService.IsRequestValidOnThisLicense(
                getMyGameRequest.License,
                getMyGameRequest.RequestorId,
                getMyGameRequest.AppId)) {

                var result = await _gamesService.GetMyGame(
                    getMyGameRequest.UserId, 
                    id,
                    getMyGameRequest.AppId,
                    fullRecord);

                if (result.Success) {

                    return Ok(result.Game);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Games/GetMyGames
        [Authorize(Roles = "USER")]
        [HttpPost, Route("GetMyGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetMyGames(
            [FromBody] GetMyGameRequest getMyGameRequest,
            [FromQuery] bool fullRecord = false) {

            if (await _appsService.IsRequestValidOnThisLicense(
                getMyGameRequest.License,
                getMyGameRequest.RequestorId,
                getMyGameRequest.AppId)) {

                var result = await _gamesService
                    .GetMyGames(getMyGameRequest.UserId, getMyGameRequest, fullRecord);

                if (result.Success) {

                    return Ok(result.Games);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Games/5/DeleteMyGame
        [Authorize(Roles = "USER")]
        [HttpDelete("{id}/DeleteMyGame")]
        public async Task<ActionResult<Game>> DeleteMyGame(
            int id,
            [FromBody] GetMyGameRequest getMyGameRequest) {

            if (await _appsService.IsRequestValidOnThisLicense(
                getMyGameRequest.License,
                getMyGameRequest.RequestorId,
                getMyGameRequest.AppId)) {
                
                var result = await _gamesService.DeleteMyGame(
                    getMyGameRequest.UserId, 
                    id);

                if (result.Success) {
                    
                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }
    }
}
