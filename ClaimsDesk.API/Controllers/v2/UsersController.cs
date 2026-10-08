using Asp.Versioning;
using ClaimsDesk.API.Data;
using ClaimsDesk.API.DTOs.v1;
using ClaimsDesk.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimsDesk.API.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Branch)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Username = u.Username,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Gender = u.Gender,
                    ReceiveNotifications = u.ReceiveNotifications,
                    Role = u.Role != null ? u.Role.Name : string.Empty,
                    Branch = u.Branch != null ? u.Branch.Name : string.Empty,
                    ApprovalLimit = u.ApprovalLimit,
                    IsActive = u.IsActive
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Branch)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                ReceiveNotifications = user.ReceiveNotifications,
                Role = user.Role != null ? user.Role.Name : string.Empty,
                Branch = user.Branch != null ? user.Branch.Name : string.Empty,
                ApprovalLimit = user.ApprovalLimit,
                IsActive = user.IsActive
            };
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(CreateUserDto createUserDto)
        {
            var user = new User
            {
                FullName = createUserDto.FullName,
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                Gender = createUserDto.Gender,
                ReceiveNotifications = createUserDto.ReceiveNotifications,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                BranchId = createUserDto.BranchId,
                RoleId = createUserDto.RoleId,
                ApprovalLimit = createUserDto.ApprovalLimit,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _context.Entry(user).Reference(u => u.Role).LoadAsync();
            await _context.Entry(user).Reference(u => u.Branch).LoadAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                ReceiveNotifications = user.ReceiveNotifications,
                Role = user.Role != null ? user.Role.Name : string.Empty,
                Branch = user.Branch != null ? user.Branch.Name : string.Empty,
                ApprovalLimit = user.ApprovalLimit,
                IsActive = user.IsActive
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = updateUserDto.FullName;
            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.Gender = updateUserDto.Gender;
            user.ReceiveNotifications = updateUserDto.ReceiveNotifications;
            user.BranchId = updateUserDto.BranchId;
            user.RoleId = updateUserDto.RoleId;
            user.ApprovalLimit = updateUserDto.ApprovalLimit;
            user.IsActive = updateUserDto.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
