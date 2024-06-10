using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetHomeProfile(int idUser)
        {
            var response = await profileService.GetUserHomeProfile(idUser);

            return new JsonResult(response);
        }

        // Удалить хуйню
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllRequestsOrganazation(int idOrganization)
        {
            var response = await profileService.GetAllRequestsOrganization(idOrganization);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUserPofile(int idUser)
        {
            var response = await profileService.GetUserProfile(idUser);

            return new JsonResult(response);
        }
        
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetOrganizationPofile(int idUser)
        {
            var response = await profileService.GetOrganizationProfile(idUser);

            return new JsonResult(response);
        }


        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetStudentPofile(int idUser)
        {
            var response = await profileService.GetStudentProfile(idUser);

            return new JsonResult(response);
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeRequest(ChangeRequest request)
        {
            var response = await profileService.UpdateRequest(request);

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddOrderFileToRequest(AddOrderFileRequest request)
        {
            var response = await profileService.UploadFileRequest(request);

            return new JsonResult(response);
        }
    }
}
