using AlmostNewOutpost.Api.Data;
using AlmostNewOutpost.Api.Models.Responses;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmostNewOutpost.Api.Repositories
{
    public class AdminRolesRepository : IAdminRolesRepository
    {
        private readonly OutpostDbContext _db;
        public AdminRolesRepository(OutpostDbContext db)
        {
            _db = db;
        }
        public async Task<List<RoleDto>> GetAll()
        {
            List<RoleDto> roles = await _db.Roles
                .OrderBy(role => role.RoleId)
                .Select(role => new RoleDto { RoleId = role.RoleId, RoleName = role.RoleName })
                .ToListAsync();

            return(roles);
        }
    }
}
