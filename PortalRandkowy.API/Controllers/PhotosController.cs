using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Dtos;
using PortalRandkowy.API.Helpers;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Controllers
{
    [Authorize]
    [Route ("api/users/{userId}/photos")]
    [ApiController]
    public class  PhotosController : ControllerBase
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary cloudinary;
        public PhotosController(IUserRepository repository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                this.cloudinaryConfig.Value.CloudName,
                this.cloudinaryConfig.Value.ApiKey,
                this.cloudinaryConfig.Value.ApiSecret
            );
            
            this.cloudinary = new Cloudinary(account);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await this.repository.GetUser(userId);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using ( var stream = file.OpenReadStream()) 
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = this.cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = this.mapper.Map<Photo>(photoForCreationDto);
            if (!userFromRepo.Photos.Any(p => p.IsMain))
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);

            if (await this.repository.SaveAll())
            {
                var photoToReturn = this.mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id}, photoToReturn);
            }      

            return BadRequest("Nie można dodać zdjęcia");
        } 

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await this.repository.GetPhoto(id);

            var photoForReturn = this.mapper.Map<PhotoForReturnDto>(photoFromRepo);
            
            return Ok(photoForReturn);
        }
    }  
}