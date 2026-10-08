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
    public class OrganizationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrganizationsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all Organizations.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetOrganizations()
        {
            var orgs = await _context.Organizations
                .Select(o => new OrganizationDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    LicenseNumber = o.LicenseNumber,
                    Slug = o.Slug,
                    CurrencyCode = o.CurrencyCode,
                    Country = o.Country,
                    Tin = o.Tin,
                    Address = o.Address,
                    AccountType = o.AccountType,
                    IsActive = o.IsActive
                })
                .ToListAsync();

            return Ok(orgs);
        }

        /// <summary>
        /// Retrieves a specific Organization by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationDto>> GetOrganization(int id)
        {
            var o = await _context.Organizations.FindAsync(id);

            if (o == null)
            {
                return NotFound();
            }

            return new OrganizationDto
            {
                Id = o.Id,
                Name = o.Name,
                LicenseNumber = o.LicenseNumber,
                Slug = o.Slug,
                CurrencyCode = o.CurrencyCode,
                Country = o.Country,
                Tin = o.Tin,
                Address = o.Address,
                AccountType = o.AccountType,
                IsActive = o.IsActive
            };
        }

        /// <summary>
        /// Retrieves a list of all OrganizationMembers.
        /// </summary>
        [HttpGet("{id}/members")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetOrganizationMembers(int id)
        {
            var orgExists = await _context.Organizations.AnyAsync(o => o.Id == id);
            if (!orgExists) return NotFound();

            var members = await _context.Members
                .Include(m => m.Customer)
                .Include(m => m.Organization)
                .Where(m => m.OrganizationId == id)
                .Select(m => new MemberDto
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
        /// Creates a new Organization.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrganizationDto>> PostOrganization(CreateOrganizationDto dto)
        {
            var org = new Organization
            {
                Name = dto.Name,
                LicenseNumber = dto.LicenseNumber,
                Slug = dto.Slug,
                CurrencyCode = dto.CurrencyCode,
                Country = dto.Country,
                Tin = dto.Tin,
                Address = dto.Address,
                AccountType = dto.AccountType,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Organizations.Add(org);
            await _context.SaveChangesAsync();

            var returnDto = new OrganizationDto
            {
                Id = org.Id,
                Name = org.Name,
                LicenseNumber = org.LicenseNumber,
                Slug = org.Slug,
                CurrencyCode = org.CurrencyCode,
                Country = org.Country,
                Tin = org.Tin,
                Address = org.Address,
                AccountType = org.AccountType,
                IsActive = org.IsActive
            };

            return CreatedAtAction(nameof(GetOrganization), new { id = org.Id }, returnDto);
        }

        /// <summary>
        /// Updates an existing Organization.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganization(int id, UpdateOrganizationDto dto)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            org.Name = dto.Name;
            org.LicenseNumber = dto.LicenseNumber;
            org.Slug = dto.Slug;
            org.CurrencyCode = dto.CurrencyCode;
            org.Country = dto.Country;
            org.Tin = dto.Tin;
            org.Address = dto.Address;
            org.AccountType = dto.AccountType;
            org.IsActive = dto.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific Organization.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            _context.Organizations.Remove(org);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganizationExists(int id)
        {
            return _context.Organizations.Any(e => e.Id == id);
        }
    }
}
