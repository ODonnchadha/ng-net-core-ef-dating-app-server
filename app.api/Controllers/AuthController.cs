﻿using app.api.DTOs;
using app.api.Entities;
using app.api.Interfaces.Respositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace app.api.Controllers
{
    [ApiController(), Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthController> logger;
        private readonly IMapper mapper;

        public AuthController(
            IAuthRepository repository, 
            IConfiguration configuration, 
            ILogger<AuthController> logger,
            IMapper mapper)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister dto)
        {
            dto.Username = dto.Username.ToLower();

            if (await repository.UserExists(dto.Username))
            {
                return BadRequest("This username already exists");
            }

            var entity = mapper.Map<User>(dto);

            var user = await repository.Register(entity, dto.Password);

            return CreatedAtRoute("GetUser",
                new { controller = "Users", id = user.Id }, mapper.Map<UserForDetails>(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin userForLogin)
        {
            var user = await repository.Login(userForLogin.Username, userForLogin.Password);

            if (null == user) return Unauthorized();

            var claims = new[] 
            { 
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new 
            { 
                token = tokenHandler.WriteToken(token),
                user = mapper.Map<UserForList>(user)
            });
        }
    }
}
