using Data.Repositary;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquiriesController : ControllerBase
    {
        private readonly IInquiries _inquiriesService;

        public InquiriesController(IInquiries inquiriesService)
        {
            _inquiriesService = inquiriesService;
        }


        [HttpPost]
        public async Task<IActionResult> InsertInquiries([FromBody] Inquiries inquiries)
        {


            var result = await _inquiriesService.InsertInquiries(inquiries);
            return Ok(result != null);
        }

    }
}
