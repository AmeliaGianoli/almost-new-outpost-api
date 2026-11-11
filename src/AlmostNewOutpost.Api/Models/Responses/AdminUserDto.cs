namespace AlmostNewOutpost.Api.Models.Responses
{
/// <summary>
/// An admin user who is signed in including their ID, username, role ID, and role
/// </summary>
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime? ArchivedAtUtc { get; set; }
    }
}
