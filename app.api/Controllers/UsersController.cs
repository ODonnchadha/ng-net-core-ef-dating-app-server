using app.api.Interfaces.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace app.api.Controllers
{
    [ApiController(), Authorize(), Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IDatingRepository repository;
        private readonly ILogger<UsersController> logger;

        public UsersController(IConfiguration configuration, IDatingRepository repository, ILogger<UsersController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet()]
        public async Task<IActionResult> GetUsers()
        {
            var users = await repository.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await repository.GetUser(id);
            return Ok(user);
        }
    }
}
