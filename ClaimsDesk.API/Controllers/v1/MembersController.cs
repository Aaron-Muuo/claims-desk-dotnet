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
    public class MembersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MembersController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all Members.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers([FromQuery] int? organizationId)
        {
            var query = _context.Members
                .Include(m => m.Customer)
                .Include(m => m.Organization)
                .AsQueryable();

            if (organizationId.HasValue)
            {
                query = query.Where(m => m.OrganizationId == organizationId.Value);
            }

            var members = await query.Select(m => new MemberDto
            {
                Id = m.Id,
                OrganizationId = m.OrganizationId,
                OrganizationName = m.Organization != null ? m.Organization.Name : string.Empty,
                CustomerId = m.CustomerId,
                CustomerName = m.Customer != null ? m.Customer.FullName : string.Empty,
                CustomerEmail = m.Customer != null ? m.Customer.Email : string.Empty,
                CustomerNationalId = m.Customer != null ? m.Customer.NationalId : string.Empty,
                MemberNumber = m.MemberNumber,
                Status = m.Status,
                JoinedDate = m.JoinedDate,
                CreatedAt = m.CreatedAt
            }).ToListAsync();

            return Ok(members);
        }

        /// <summary>
        /// Retrieves a specific Member by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMember(int id)
        {
            var m = await _context.Members
                .Include(m => m.Customer)
                .Include(m => m.Organization)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (m == null) return NotFound();

            return new MemberDto
            {
                Id = m.Id,
                OrganizationId = m.OrganizationId,
                OrganizationName = m.Organization != null ? m.Organization.Name : string.Empty,
                CustomerId = m.CustomerId,
                CustomerName = m.Customer != null ? m.Customer.FullName : string.Empty,
                CustomerEmail = m.Customer != null ? m.Customer.Email : string.Empty,
                CustomerNationalId = m.Customer != null ? m.Customer.NationalId : string.Empty,
                MemberNumber = m.MemberNumber,
                Status = m.Status,
                JoinedDate = m.JoinedDate,
                CreatedAt = m.CreatedAt
            };
        }

        /// <summary>
        /// Creates a new Member.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MemberDto>> PostMember(CreateMemberDto dto)
        {
            var m = new Member
            {
                OrganizationId = dto.OrganizationId,
                CustomerId = dto.CustomerId,
                MemberNumber = dto.MemberNumber,
                Status = dto.Status,
                JoinedDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.Members.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMember), new { id = m.Id }, new MemberDto
            {
                Id = m.Id,
                OrganizationId = m.OrganizationId,
                CustomerId = m.CustomerId,
                MemberNumber = m.MemberNumber,
                Status = m.Status,
                JoinedDate = m.JoinedDate,
                CreatedAt = m.CreatedAt
            });
        }
    }
}
