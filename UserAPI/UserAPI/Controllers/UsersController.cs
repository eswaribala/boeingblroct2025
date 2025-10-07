using Microsoft.AspNetCore.Mvc;
using UserAPI.Services;
using AutoMapper;
using UserAPI.DTOS;
using UserAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, AutoMapper.IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        // GET: api/<UsersController>
        [HttpGet]
        public Task<IEnumerable<IActionResult>> Get()
        {
            var users =  _userService.GetAllUsers();
            var userReadDTOs = _mapper.Map<IEnumerable<BoeingUserReadDTO>>(users);
            return Task.FromResult(userReadDTOs.Select(dto => (IActionResult)Ok(dto)));

        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
           var user = await  _userService.GetUserById(id);
           return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BoeingUserDTO boeingUserDTO)
        {
            var boeingUser = _mapper.Map<BoeingUser>(boeingUserDTO);
            var createdUser = await _userService.AddUser(boeingUser);
            var boeingUserReadDTO = _mapper.Map<BoeingUserReadDTO>(createdUser);
            return Ok(boeingUserReadDTO);
            
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BoeingUserDTO boeingUserDTO )
        {
            var boeingUser = _mapper.Map<BoeingUser>(boeingUserDTO);
            boeingUser.UserId = id;
            var updatedUser =  await _userService.UpdateUser(boeingUser);
            return Ok(updatedUser);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
