using app.api.DTOs;
using app.api.Entities;
using app.api.Extensions;
using app.api.Filters;
using app.api.Helpers.Messaging;
using app.api.Interfaces.Respositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace app.api.Controllers
{
    [ApiController(), Authorize(), Route("api/users/{userId}/[controller]"), ServiceFilter(typeof(LogUserActivity))]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository repository;
        private readonly IMapper mapper;
        public MessagesController(IDatingRepository repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var entity = await repository.GetMessage(id);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpGet("threads/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var entities = await repository.GetMessageThread(userId, recipientId);
            var dto = mapper.Map<IEnumerable<MessageToReturn>>(entities);

            return Ok(dto);
        }

        /// <summary>
        /// /api/users/40/messages?messageContainer=Inbox
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="messageParams"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            messageParams.UserId = userId;
            var entities = await repository.GetMessagesForUser(messageParams);
            var dto = mapper.Map<IEnumerable<MessageToReturn>>(entities);

            Response.AddPagination(
                entities.CurrentPage, entities.PageSize, entities.TotalCount, entities.TotalPages);

            return Ok(dto);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreation dto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            dto.SenderId = userId;

            var recipient = await repository.GetUser(dto.RecipientId);
            if (recipient == null)
            {
                return BadRequest("Recipient does not exist");
            }

            var message = mapper.Map<Message>(dto);

            repository.Add(message);

            if (await repository.SaveAll())
            {
                return CreatedAtRoute("GetMessage", 
                    new { id = message.Id }, mapper.Map<MessageForCreation>(message));
            }

            throw new Exception("Message creation failed on save");
        }
    }
}
