using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.RequestModels;

namespace SudokuCollective.Api.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private readonly IAppsService appsService;

        public LicensesController(IAppsService appsServ)
        {
            appsService = appsServ;
        }

        // GET: api/licenses/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet, Route("{id}")]
        public async Task<ActionResult> GetLicense(int id)
        {
            var result = await appsService.GetLicense(id);

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

        // POST: api/licenses
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<App>> PostApp(
            [FromBody] LicenseRequest request)
        {
            var result = await appsService.CreateApp(request);

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
