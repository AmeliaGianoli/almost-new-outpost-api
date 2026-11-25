using AlmostNewOutpost.Api.Models.Responses;
using AlmostNewOutpost.Api.Models.Entities;

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

        public async Task<UserOperationStatus> ArchivedAsync(int userId)
        {
            User? user = await _users.FindByIdAsync(userId);
            if (user == null)
            {
                return UserOperationStatus.NotFound;
            }
            if (user.ArchivedAtUtc != null)
            {
                return UserOperationStatus.AlreadyArchived;
            }
            user.ArchivedAtUtc = DateTime.UtcNow;
            await _users.SaveChangesAsync();
            return UserOperationStatus.Success;
        }

        public async Task<UserOperationStatus> RestoreAsync(int userId)
        {
            User? user = await _users.FindByIdAsync(userId);
            if (user == null)
            {
                return UserOperationStatus.NotFound;
            }
            if (user.ArchivedAtUtc == null)
            {
                return UserOperationStatus.NotArchived;
            }
            user.ArchivedAtUtc = null;
            await _users.SaveChangesAsync();
            return UserOperationStatus.Success;
        }

    }
}
