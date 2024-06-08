using Microsoft.AspNetCore.Mvc;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Controllers
{
    // надо бы сделать просто файл сервис где буду отдаваться файлы а то что то тут это позорище которое плодит много кода
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IProfileService profileService;

        public FileController(IAccountService accountService, IProfileService profileService)
        {
            this.accountService = accountService;
            this.profileService = profileService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserImage(int userId)
        {
            var bytes = await accountService.GetImageUser(userId);

            return File(bytes, "image/png");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderFile(int allocationRequestId)
        {
            var bytes = await profileService.GetOrderAllocationRequest(allocationRequestId);

            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }
    }
}
