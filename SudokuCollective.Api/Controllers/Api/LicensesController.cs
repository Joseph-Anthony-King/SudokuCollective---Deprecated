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
                result.Message = "Status Code 200: License Obtained";

                return Ok(result);
            }
            else
            {
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
                result.Message = "Status Code 200: App Created";

                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    }
}
