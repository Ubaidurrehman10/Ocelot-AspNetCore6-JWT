using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poc.FirstApi.Web.Model;

namespace Poc.FirstApi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("its working");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] CartItem cartItem)
        {
            return Ok("its working");
        }

        //// PUT api/<CartController>
        //[HttpPut]
        //[Authorize]
        //public IActionResult Put([FromQuery(Name = "u")] string userId, [FromBody] CartItem cartItem)
        //{
        //    cartRepository.UpdateCartItem(userId, cartItem);
        //    return Ok();
        //}

        //// DELETE api/<CartController>
        //[HttpDelete]
        //[Authorize]
        //public IActionResult Delete([FromQuery(Name = "u")] string userId, [FromQuery(Name = "ci")] string cartItemId)
        //{
        //    cartRepository.DeleteCartItem(userId, cartItemId);
        //    return Ok();
        //}

        //// PUT api/<CartController>/update-catalog-item
        //[HttpPut("update-catalog-item")]
        //[Authorize]
        //public IActionResult Put([FromQuery(Name = "ci")] string catalogItemId, [FromQuery(Name = "n")] string name, [FromQuery(Name = "p")] decimal price)
        //{
        //    cartRepository.UpdateCatalogItem(catalogItemId, name, price);
        //    return Ok();
        //}

        //// DELETE api/<CartController>/delete-catalog-item
        //[HttpDelete("delete-catalog-item")]
        //[Authorize]
        //public IActionResult Delete([FromQuery(Name = "ci")] string catalogItemId)
        //{
        //    cartRepository.DeleteCatalogItem(catalogItemId);
        //    return Ok();
        //}
    }
}
