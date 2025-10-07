using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOS;
using UserAPI.Models;
using UserAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserAPI.Controllers
{
    [ApiVersion("1.0")]
    
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RolesController(IRoleService roleService, AutoMapper.IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }


        // GET: api/<RolesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleReadDTO>>> Get()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(_mapper.Map<IEnumerable<RoleReadDTO>>(roles));
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var role = await _roleService.GetRoleById(id);
            return Ok(_mapper.Map<RoleReadDTO>(role)); ;
        }

        // POST api/<RolesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleDTO RoleDTO)
        {
            var role = _mapper.Map<Role>(RoleDTO);
            var createdRole = await _roleService.AddRole(role);
            var roleReadDTO = _mapper.Map<RoleReadDTO>(createdRole);
            return Ok(roleReadDTO);

        }

        // PUT api/<RolesController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RoleDTO RoleDTO)
        {
            var role = _mapper.Map<Role>(RoleDTO);

            var updatedRole = await _roleService.UpdateRole(role);
            var roleUpdatedDTO = _mapper.Map<RoleReadDTO>(updatedRole);
            return Ok(roleUpdatedDTO);
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleService.DeleteRole(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
