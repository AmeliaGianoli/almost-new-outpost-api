using AlmostNewOutpost.Api.Models.Responses;
using AlmostNewOutpost.Api.Models.Entities;
namespace AlmostNewOutpost.Api.Repositories
{
    public interface IUserAdminRepository
    {
        Task<List<AdminUserDto>> GetAllAsync(bool includeArchived);
        Task<User?> FindByIdAsync(int userId);
        Task SaveChangesAsync();
    }
}
