using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IDocService docService;

        public ReportController(IDocService docService)
        {
            this.docService = docService;
        }


        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetReport(int idGroup)
        {
            var document = await docService.CreateReport(idGroup);

            using (MemoryStream memoryStream = new())
            {
                document.SaveAs(memoryStream);

                var fileBytes = memoryStream.ToArray();

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Allocation.docx");
            }
        }
    }
}
