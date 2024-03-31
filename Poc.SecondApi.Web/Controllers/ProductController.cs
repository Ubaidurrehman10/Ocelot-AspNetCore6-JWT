using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Poc.SecondApi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        public ProductController()
        {
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("It's Working now!");
        }
    }
}
