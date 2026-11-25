using AlmostNewOutpost.Api.Models.Responses;

namespace AlmostNewOutpost.Api.Services
{
    public interface IAdminRolesService
    {
        Task<List<RoleDto>> GetAll();
    }
}
