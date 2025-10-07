using Microsoft.AspNetCore.Mvc;
using UserAPI.Services;
using AutoMapper;
using UserAPI.DTOS;
using UserAPI.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors]
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
        public async Task<ActionResult<IEnumerable<BoeingUserReadDTO>>> Get()
        {
            var users = await _userService.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<BoeingUserReadDTO>>(users));
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
           var user = await  _userService.GetUserById(id);
           return Ok(_mapper.Map<BoeingUserReadDTO>(user));
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
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BoeingUserDTO boeingUserDTO )
        {
            var boeingUser = _mapper.Map<BoeingUser>(boeingUserDTO);
       
            var updatedUser =  await _userService.UpdateUser(boeingUser);
            var boeingUserUpdatedDTO = _mapper.Map<BoeingUserReadDTO>(updatedUser);
            return Ok(boeingUserUpdatedDTO);
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
