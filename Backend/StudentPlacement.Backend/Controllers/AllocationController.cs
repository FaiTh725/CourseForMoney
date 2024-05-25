using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPlacement.Backend.Migrations;
using StudentPlacement.Backend.Models.Allocation;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AllocationController : ControllerBase
    {
        private readonly IAllocationService allocationService;

        public AllocationController(IAllocationService allocationService)
        {
            this.allocationService = allocationService;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetDepartmentsAndGroups()
        {
            var response = await allocationService.GetAllDepartmentsAndGroups();

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllRequests()
        {
            var response = await allocationService.GetAllAllocations();

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUsers(int idGroup)
        {
            var response = await allocationService.GetStudents(idGroup);

            return new JsonResult(response);
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> AddAllocationRequest(AddDeleteRequestToUserRequest request)
        {
            var response = await allocationService.AddRequestToStudent(request);

            return new JsonResult(response);
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteAllocationRequest(AddDeleteRequestToUserRequest request)
        {
            var response = await allocationService.DeleteRequestStudent(request);

            return new JsonResult(response);
        }
    }
}
