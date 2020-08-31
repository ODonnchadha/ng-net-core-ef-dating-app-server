using app.api.DTOs;
using app.api.Interfaces.Respositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.api.Controllers
{
    [ApiController(), Authorize(), Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository repository;
        private readonly ILogger<UsersController> logger;
        private readonly IMapper mapper;

        public UsersController(IDatingRepository repository, ILogger<UsersController> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet()]
        public async Task<IActionResult> GetUsers()
        {
            var users = await repository.GetUsers();

            var dto = mapper.Map<IEnumerable<UserForList>>(users);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await repository.GetUser(id);

            var dto = mapper.Map<UserForDetails>(user);

            return Ok(dto);
        }
    }
}
