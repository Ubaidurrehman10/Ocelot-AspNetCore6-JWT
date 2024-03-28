using Microsoft.AspNetCore.Mvc;
using Poc.Auth.Web.Model;
using Poc.Middleware.Auth;

namespace Poc.Auth.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly IJwtBuilder _jwtBuilder;

        public IdentityController(ILogger<IdentityController> logger, IJwtBuilder jwtBuilder)
        {
            _logger = logger;
            _jwtBuilder = jwtBuilder;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var token = _jwtBuilder.GetToken(user.UserId.ToString());

            return Ok(token);
        }
    }
}
