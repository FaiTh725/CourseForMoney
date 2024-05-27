using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Models.Account;
using StudentPlacement.Backend.Models.Enter;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService enterService;

        public AccountController(IAccountService enterService)
        {
            this.enterService = enterService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Enter(EnterRequest request)
        {
            var response = await enterService.Enter(request);

            if (response.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                Response.Cookies.Append("RefreshToken", response.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(3)
                });
            }

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
        {
            var response = await enterService.CreateAccount(request);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetStudentSetting()
        {
            var resposen = await enterService.GetStudentSetting();

            return new JsonResult(resposen);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await enterService.GetAllUsers();

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUser(int idUser)
        {
            var response = await enterService.GetUser(idUser);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> RefreshToken()
        {
            var response = await enterService.RefreshToken(Request.Cookies["RefreshToken"]);
            
            Response.Cookies.Delete("RefreshToken");
            
            if(response.StatusCode == 0)
            {

                Response.Cookies.Append("RefreshToken", response.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(3)
                });
            }


            return new JsonResult(response);
        }


        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(DeleteUserRequest request)
        {
            var response = await enterService.DeleteUser(request);

            return new JsonResult(response);
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeUser(ChangeUserRequest request)
        {
            var response = await enterService.ChangeUser(request);

            return new JsonResult(response);
        }
    }
}
