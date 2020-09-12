using app.api.DTOs;
using app.api.Interfaces.Respositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
            var entities = await repository.GetUsers();

            var dtos = mapper.Map<IEnumerable<UserForList>>(entities);

            return Ok(dtos);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var entity = await repository.GetUser(id);

            var dto = mapper.Map<UserForDetails>(entity);

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdate dto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var entity = await repository.GetUser(id);

            mapper.Map(dto, entity);

            if (await repository.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user {id} failed on save");
        }
    }
}
