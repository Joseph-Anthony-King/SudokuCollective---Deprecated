using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Api.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase {

        private readonly IAppsService _appsService;

        public LicensesController(IAppsService appsService) {

            _appsService = appsService;
        }
        
        // GET: api/Licenses/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet, Route("{id}")]
        public async Task<ActionResult> GetLicense(int id) {
            
            var result = await _appsService.GetLicense(id);

            if (result.Success) {
                
                return Ok(result.License);

            } else {

                return NotFound(result.Message);
            }
        }
        
        // POST: api/Licenses
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<App>> PostApp(
            [FromBody] LicenseRequest licenseRequest) {
            
            var result = await _appsService.CreateApp(licenseRequest);

            if (result.Success) {
                
                return Ok(result.App);

            } else {

                return NotFound(result.Message);
            }
        }
    }
}
