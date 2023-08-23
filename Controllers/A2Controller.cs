using A2.Data;
using A2.Models;
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
            if (await _repository.FindUser(user.UserName))
            {
                return Ok($"UserName {user.UserName} is not available.");
            }

            await _repository.RegisterUser(user);

            return Ok("User successfully registered.");
        }
    }
}