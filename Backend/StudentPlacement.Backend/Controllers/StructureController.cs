using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPlacement.Backend.Models.Structure;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StructureController : ControllerBase
    {
        private readonly IStructureService structureService;

        public StructureController(IStructureService structureService)
        {
            this.structureService = structureService;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments()
        {
            var response = await structureService.GetAllDepartments();

            return new JsonResult(response);    
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllSpecialities()
        {
            var response = await structureService.GetAllCpecialities();

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateGroup(CreateGroupRequest request)
        {
            var response = await structureService.CreateGroup(request);

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateSpeciality(CreateSpecialityRequest request)
        {
            var response = await structureService.CreateSpeciality(request);

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentRequest request)
        {
            var response = await structureService.CreateDepartment(request);
            
            return new JsonResult(response);
        }
    }
}
