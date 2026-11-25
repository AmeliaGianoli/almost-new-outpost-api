using AlmostNewOutpost.Api.Models.Responses;
namespace AlmostNewOutpost.Api.Repositories
{
    public interface IAdminRolesRepository
    {
        Task<List<RoleDto>> GetAll();
    }
}
