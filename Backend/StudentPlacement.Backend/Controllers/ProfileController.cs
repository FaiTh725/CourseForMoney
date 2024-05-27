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
            var response = await profileService.AddAllocationRequest(request);

            return new JsonResult(response);
        }

        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteAllocationRequest(DeleteAllocationRequest request)
        {
            var response = await profileService.DeleteAllocationRequest(request);

            return new JsonResult(response);
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeProfile(ChangeProfileRequest request)
        {
            var response = await profileService.UpdateProfileOrganization(request);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetStudentRequest(int idUser)
        {
            var response = await profileService.GetStudentRequest(idUser);

            return new JsonResult(response);
        }
    }
}
