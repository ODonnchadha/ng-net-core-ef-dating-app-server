using app.api.DTOs;
using app.api.Entities;
using app.api.Interfaces.Respositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace app.api.Controllers
{
    [ApiController(), Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuthRepository repository, ILogger<AuthController> logger)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister userForRegister)
        {
            userForRegister.Username = userForRegister.Username.ToLower();

            if (await repository.UserExists(userForRegister.Username))
            {
                return BadRequest("This username already exists");
            }

            var user = await repository.Register(
                new User { Username = userForRegister.Username }, userForRegister.Password);

            return StatusCode(201);
        }
    }
}
