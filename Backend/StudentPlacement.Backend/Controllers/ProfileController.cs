using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPlacement.Backend.Models.Profile;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController: ControllerBase
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddAllocationRequest(AddAllocationRequestRequest request)
        {
            var response = profileService.AddAllocationRequest(request);

            return new JsonResult(response);
        }
    }
}
