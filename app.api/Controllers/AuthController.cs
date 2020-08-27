using app.api.Interfaces.Respositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
    }
}
