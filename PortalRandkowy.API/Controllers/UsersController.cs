using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Dtos;

namespace PortalRandkowy.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository repo;
        private readonly IMapper mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
                var users = await this.repo.GetUsers();
                var usersToReturn = this.mapper.Map<IEnumerable<UserForListDto>>(users);
                return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await this.repo.GetUser(id);
            var userToReturn = this.mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await this.repo.GetUser(id);
            mapper.Map(userForUpdateDto, userFromRepo);
            if (await this.repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Aktualizacja użytkownika o id: {id} nie powiodła się przy zapisywaniu do bazy");
        }
    }
}