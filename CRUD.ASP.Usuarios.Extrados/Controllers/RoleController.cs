using AccesData.DTOs.RoleDTOs;
using AccesData.InputsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("create")]

        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest roleRequest)
        {


            try
            {


                CreateRoleDTO roleCreated = await _roleService.CreateRoleService(roleRequest);
                if (roleCreated.msg == "The role is already in use") return Conflict("The role is already in use");
                if (roleCreated.msg == "server error") return StatusCode(500, "server error");
                return Ok(roleCreated);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new role {Ex.Message}");
                return StatusCode(500, "server error:");
            }


        }




    }

}
