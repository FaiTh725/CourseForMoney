using Microsoft.AspNetCore.Mvc;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IAccountService accountService;

        public ImageController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserImage(int userId)
        {
            var bytes = await accountService.GetImageUser(userId);

            return File(bytes, "image/png");
        }
    }
}
