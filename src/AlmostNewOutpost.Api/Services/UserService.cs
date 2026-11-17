using AlmostNewOutpost.Api.Models.Responses;
using AlmostNewOutpost.Api.Repositories;

namespace AlmostNewOutpost.Api.Services
{
    public class UserService
    {
        private readonly IUserAdminRepository _users;
        public UserService(IUserAdminRepository users)
        {
            _users = users;
        }

        public async Task<List<AdminUserDto>> GetAllAsync(bool includeArchived)
        {
            return await _users.GetAllAsync(includeArchived);
        }

    }
}
