using app.api.DTOs;
using app.api.Entities;
using app.api.Interfaces.Respositories;
using app.api.Settings;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace app.api.Controllers
{
    [ApiController(), Authorize(), Route("api/users/{userId}/[controller]")]
    public class PhotosController : ControllerBase
    {
        private Cloudinary cloudinary;
        private readonly IDatingRepository repository;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> options;
        public PhotosController(IDatingRepository repository, IMapper mapper, IOptions<CloudinarySettings> options)
        {
            this.mapper = mapper;
            this.options = options;
            this.repository = repository;

            this.cloudinary = new Cloudinary(new Account
            {
                Cloud = options.Value.CloudName,
                ApiKey = options.Value.ApiKey,
                ApiSecret = options.Value.ApiSecret,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await repository.GetUser(userId);

            if ((bool)!user?.Photos?.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photo = await repository.GetPhoto(id);

            if ((bool)photo?.IsDefault)
            {
                return BadRequest("Cannot delete default photo.");
            }

            if (photo.PublicId != null)
            {
                // response.Result == "ok"
                cloudinary.Destroy(new DeletionParams(photo.PublicId));
            }

            repository.Delete(photo);

            if (await repository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Error deleting photo");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var entity = await repository.GetPhoto(id);
            var dto = mapper.Map<PhotoForReturn>(entity);

            return Ok(dto);
        }

        [HttpPost()]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreation dto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await repository.GetUser(userId);

            var file = dto.File;

            var uploadResult = new ImageUploadResult { };

            if (file?.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            dto.Url = uploadResult.Url.ToString();
            dto.PublicId = uploadResult.PublicId;

            var photo = mapper.Map<Photo>(dto);

            if (!user.Photos.Any(p => p.IsDefault))
            {
                photo.IsDefault = true;
            }

            user.Photos.Add(photo);

            if (await repository.SaveAll())
            {
                var p = mapper.Map<PhotoForReturn>(photo);
                return Ok(p);
            }

            return BadRequest("Unable to upload photo");
        }

        [HttpPost("{id}/default")]
        public async Task<IActionResult> SetDefaultPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await repository.GetUser(userId);

            if ((bool)!user?.Photos?.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photo = await repository.GetPhoto(id);

            if (photo.IsDefault)
            {
                return BadRequest("This is already the default photo");
            }

            var current = await repository.GetDefaultPhoto(userId);

            current.IsDefault = false;
            photo.IsDefault = true;

            if (await repository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Error setting default photo");
        }
    }
}
