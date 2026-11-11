namespace AlmostNewOutpost.Api.Models.Responses
{
    /// <summary>
    /// the user role and corresponding ID
    /// </summary>
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
