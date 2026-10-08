using Asp.Versioning;
using ClaimsDesk.API.Data;
using ClaimsDesk.API.DTOs.v1;
using ClaimsDesk.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimsDesk.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserLicensesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserLicensesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all UserLicenses.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLicenseDto>>> GetUserLicenses()
        {
            var data = await _context.UserLicenses
                .Select(u => new UserLicenseDto
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    LicensePoolId = u.LicensePoolId,
                    AssignedAt = u.AssignedAt
                }).ToListAsync();
            return Ok(data);
        }

        /// <summary>
        /// Retrieves a specific UserLicense by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLicenseDto>> GetUserLicense(int id)
        {
            var u = await _context.UserLicenses.FindAsync(id);
            if (u == null) return NotFound();

            return new UserLicenseDto
            {
                Id = u.Id,
                UserId = u.UserId,
                LicensePoolId = u.LicensePoolId,
                AssignedAt = u.AssignedAt
            };
        }

        /// <summary>
        /// Creates a new UserLicense.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserLicenseDto>> PostUserLicense(CreateUserLicenseDto dto)
        {
            var u = new UserLicense
            {
                UserId = dto.UserId,
                LicensePoolId = dto.LicensePoolId,
                AssignedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.UserLicenses.Add(u);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserLicense), new { id = u.Id }, new UserLicenseDto
            {
                Id = u.Id,
                UserId = u.UserId,
                LicensePoolId = u.LicensePoolId,
                AssignedAt = u.AssignedAt
            });
        }

        /// <summary>
        /// Updates an existing UserLicense.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLicense(int id, UpdateUserLicenseDto dto)
        {
            var u = await _context.UserLicenses.FindAsync(id);
            if (u == null) return NotFound();

            u.UserId = dto.UserId;
            u.LicensePoolId = dto.LicensePoolId;
            u.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific UserLicense.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserLicense(int id)
        {
            var u = await _context.UserLicenses.FindAsync(id);
            if (u == null) return NotFound();

            _context.UserLicenses.Remove(u);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
