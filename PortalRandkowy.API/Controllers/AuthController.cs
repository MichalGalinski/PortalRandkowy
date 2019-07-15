using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Dtos;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        public AuthController(IAuthRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if(await this.repository.UserExist(userForRegisterDto.Username))
                return BadRequest("Użytownik o takiej nazwie już istnieje !");
            var userToCreate = new User
            {
                UserName = userForRegisterDto.Username
            };
            var createdUser = await this.repository.Register(userToCreate,userForRegisterDto.Password);
            return StatusCode(201);
        }
    }
}