using Microsoft.AspNetCore.Mvc;
using static TrabalhoBackEnd.Models.Roles;
using TrabalhoBackEnd.BLL;
using TrabalhoBackEnd.Models.Requests;
using TrabalhoBackEnd.Models.Responses;
using Microsoft.AspNetCore.Authorization;

namespace TrabalhoBackEnd.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public IActionResult CreateRole(CreateRoleRequest roleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = roleRequest.ToRole();
            _roleService.AddRole(role);

            return Ok("Role created successfully.");
        }

        [HttpGet]
        public ActionResult<List<Role>> GetAllRoles()
        {
            return _roleService.GetAllRoles();
        }

        [HttpGet("{id}")]
        public ActionResult<RoleResponse> GetRoleById(int id)
        {
            try
            {
                var role = _roleService.GetRoleById(id);
                var response = new RoleResponse(role);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
