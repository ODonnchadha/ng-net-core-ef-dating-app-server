using app.api.DTOs;
using app.api.Extensions;
using app.api.Filters;
using app.api.Helpers.Users;
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
    [ApiController(), Authorize(), Route("api/[controller]"), ServiceFilter(typeof(LogUserActivity))]
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

        /// <summary>
        /// /api/users?pageNumber=1&pageSize=10
        /// pagination: {currentPage":1,"itemsPerPage":10,"totalItems":13,"totalPages":2}
        /// </summary>
        /// <param name="userParams"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var user = await repository.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            userParams.UserId = user.Id;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = user.Gender == "male" ? "female" : "male";
            }

            var entity = await repository.GetUsers(userParams);

            Response.AddPagination(
                entity.CurrentPage, entity.PageSize, entity.TotalCount, entity.TotalPages);

            var dtos = mapper.Map<IEnumerable<UserForList>>(entity);

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
