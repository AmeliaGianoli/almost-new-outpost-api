using AlmostNewOutpost.Api.Data;
using AlmostNewOutpost.Api.Models.Responses;
using AlmostNewOutpost.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AlmostNewOutpost.Api.Controllers.Admin
{
    /// <summary>
    /// retrieves roles by role ID and role name
    /// </summary>
    [ApiController]
    [Route("api/admin/roles")]
    public class AdminRolesController : ControllerBase
    {
        //private readonly OutpostDbContext _db;
        private readonly IAdminRolesService _roles;
        public AdminRolesController(IAdminRolesService rolesService)
        {
            //_db = db;
            _roles = rolesService;
        }

        

        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> GetAll()
        {

            List<RoleDto> roles = await _roles.GetAll();
            return Ok(roles);
        }

    }
}
