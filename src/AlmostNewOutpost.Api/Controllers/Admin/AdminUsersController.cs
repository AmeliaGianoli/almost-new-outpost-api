using AlmostNewOutpost.Api.Data;
using AlmostNewOutpost.Api.Models.Entities;
using AlmostNewOutpost.Api.Models.Requests;
using AlmostNewOutpost.Api.Models.Responses;
using AlmostNewOutpost.Api.Models.Rows;
using AlmostNewOutpost.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AlmostNewOutpost.Api.Controllers.Admin
{
    /// <summary>
    /// Connects each user to their role
    /// </summary>
    [ApiController]
    [Route("api/admin/users")]
    public class AdminUsersController : ControllerBase
    {
        private readonly OutpostDbContext _db;
        private readonly IPasswordHasher<UserRow> _passwordHasher;
        private readonly UserService _userService;
        public AdminUsersController(OutpostDbContext db, IPasswordHasher<UserRow> passwordHasher, UserService userService)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _userService = userService;
        }

        

        [HttpGet]
        public async Task<ActionResult<List<AdminUserDto>>> GetAll([FromQuery] bool includeArchived = false)
        {
            List<AdminUserDto> users = await _userService.GetAllAsync(includeArchived);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdminUserDto>> GetById([FromRoute] int id)
        {
            AdminUserDto? adminUser= await FindUserAsync(id);


            if (adminUser == null)
            {
                return Problem(title: "User not found.", statusCode: StatusCodes.Status404NotFound);
            }
            return Ok(adminUser);
        }

        private async Task<AdminUserDto?> FindUserAsync(int id)
        {
            AdminUserDto? adminUser = await (
                            from user in _db.Users
                            join role in _db.Roles on user.RoleId equals role.RoleId
                            where user.UserId == id
                            select new AdminUserDto
                            {
                                Id = user.UserId,
                                Username = user.Username,
                                RoleId = role.RoleId,
                                Role = role.RoleName,
                                ArchivedAtUtc = user.ArchivedAtUtc,
                            }).FirstOrDefaultAsync();
            return adminUser;

        }

        [HttpPost]
        public async Task<ActionResult<AdminUserDto>> Create(CreateUserRequest request)
        {
            string username = request.Username.Trim();

            bool usernameTaken = await _db.Users.AnyAsync(user => user.Username == username);
            if (usernameTaken)
            {
                return Problem(title: "That username is already taken.", statusCode: StatusCodes.Status409Conflict);
            }

            int? roleId = request.RoleId;
            bool RoleIdExist = await _db.Roles.AnyAsync(role => role.RoleId == roleId);
            if (!RoleIdExist)
            {
                ModelState.AddModelError(nameof(CreateUserRequest.RoleId), "Choose a role that exists.");
                return ValidationProblem(ModelState);
            }

            string passwordHash = _passwordHasher.HashPassword(new UserRow(), request.Password);
            User user = new User { Username = username, PasswordHash = passwordHash, RoleId = request.RoleId!.Value };
            _db.Users.Add(user);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException exception) when (DatabaseErrors.GetErrorNumber(exception) == DatabaseErrors.DuplicateKey)
            {
                return Problem(title: "That username is already taken.", statusCode: StatusCodes.Status409Conflict);
            }
            AdminUserDto? created = await FindUserAsync(user.UserId);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, created);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<AdminUserDto>> Update(UpdateUserRequest request, [FromRoute] int id)
        {
            User? user = await _db.Users.FindAsync(id);
            if (user is null)
            {
                return Problem(title: "User not found.", statusCode: StatusCodes.Status404NotFound);
            }
            string username = request.Username.Trim();
            if (await _db.Users.AnyAsync(other => other.Username == username && other.UserId != id))
            {
                return Problem(title: "That username is already taken.", statusCode: StatusCodes.Status409Conflict);
            }
            int roleId = request.RoleId!.Value;
            bool roleIdExist = await _db.Roles.AnyAsync(role => role.RoleId == roleId);
            if (!roleIdExist)
            {
                ModelState.AddModelError(nameof(UpdateUserRequest.RoleId), "Choose a role that exists.");
                return ValidationProblem(ModelState);
            }
            user.Username = username;
            user.RoleId = roleId;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException exception) when (DatabaseErrors.GetErrorNumber(exception) == DatabaseErrors.DuplicateKey)
            {
                return Problem(title: "That username is already taken.", statusCode: StatusCodes.Status409Conflict);
            }
            return Ok(await FindUserAsync(id)) ;


        }
        


    }
}
