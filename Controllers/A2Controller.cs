using A2.Data;
using A2.Dtos;
using A2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace A2.Controllers
{
    [ApiController]
    [Route("webapi")]
    public class A2Controller : Controller
    {
        private readonly IA2Repo _repository;

        public A2Controller(IA2Repo repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _repository.IsUserNameRegistered(user.UserName))
            {
                return Ok($"UserName {user.UserName} is not available.");
            }

            await _repository.RegisterUser(user);

            return Ok("User successfully registered.");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BasicAuthentication", Policy = "IsRegisteredUser")]
        [Route("PurchaseItem")]
        public async Task<IActionResult> PurchaseItem(int productId)
        {
            var product = await _repository.FindProduct(productId);

            if (product == null)
            {
                return BadRequest($"Product {productId} not found");
            }

            var purchaseOutput = new PurchaseOutput(User.Identity.Name, productId);

            return Ok(purchaseOutput);
        }
    }
}