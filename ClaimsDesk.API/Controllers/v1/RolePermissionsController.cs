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
    public class RolePermissionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolePermissionsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all RolePermissions.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolePermissionDto>>> GetRolePermissions([FromQuery] int? organizationId, [FromQuery] int? roleId)
        {
            var query = _context.RolePermissions.AsQueryable();

            if (organizationId.HasValue)
            {
                query = query.Where(rp => rp.OrganizationId == organizationId.Value);
            }

            if (roleId.HasValue)
            {
                query = query.Where(rp => rp.RoleId == roleId.Value);
            }

            var data = await query
                .Select(rp => new RolePermissionDto
                {
                    Id = rp.Id,
                    RoleId = rp.RoleId,
                    PermissionId = rp.PermissionId,
                    OrganizationId = rp.OrganizationId
                }).ToListAsync();

            return Ok(data);
        }

        /// <summary>
        /// Creates a new RolePermission.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RolePermissionDto>> PostRolePermission(CreateRolePermissionDto dto)
        {
            var rp = new RolePermission
            {
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId,
                OrganizationId = dto.OrganizationId
            };

            _context.RolePermissions.Add(rp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRolePermissions), new { id = rp.Id }, new RolePermissionDto
            {
                Id = rp.Id,
                RoleId = rp.RoleId,
                PermissionId = rp.PermissionId,
                OrganizationId = rp.OrganizationId
            });
        }

        /// <summary>
        /// Deletes a specific RolePermission.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolePermission(int id)
        {
            var rp = await _context.RolePermissions.FindAsync(id);
            if (rp == null) return NotFound();

            _context.RolePermissions.Remove(rp);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
