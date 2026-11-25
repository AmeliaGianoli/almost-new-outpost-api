using AlmostNewOutpost.Api.Models.Responses;
using AlmostNewOutpost.Api.Repositories;

namespace AlmostNewOutpost.Api.Services
{
    public class AdminRolesService : IAdminRolesService
    {
        private readonly IAdminRolesRepository _roles;
        public AdminRolesService(IAdminRolesRepository roles)
        {
            _roles = roles;
        }
        public async Task<List<RoleDto>> GetAll()
        {
            return await _roles.GetAll();
        }
    }
}
