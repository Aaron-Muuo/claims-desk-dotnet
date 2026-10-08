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
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all Customers.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _context.Customers
                .Include(c => c.Memberships)
                .ThenInclude(m => m.Organization)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    NationalId = c.NationalId,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt,
                    Memberships = c.Memberships.Select(m => new CustomerMembershipDto
                    {
                        MemberId = m.Id,
                        OrganizationId = m.OrganizationId,
                        OrganizationName = m.Organization != null ? m.Organization.Name : string.Empty,
                        MemberNumber = m.MemberNumber,
                        Status = m.Status,
                        JoinedDate = m.JoinedDate
                    }).ToList()
                }).ToListAsync();

            return Ok(customers);
        }

        /// <summary>
        /// Retrieves a specific Customer by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var c = await _context.Customers
                .Include(c => c.Memberships)
                .ThenInclude(m => m.Organization)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null) return NotFound();

            return new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                NationalId = c.NationalId,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                Memberships = c.Memberships.Select(m => new CustomerMembershipDto
                {
                    MemberId = m.Id,
                    OrganizationId = m.OrganizationId,
                    OrganizationName = m.Organization != null ? m.Organization.Name : string.Empty,
                    MemberNumber = m.MemberNumber,
                    Status = m.Status,
                    JoinedDate = m.JoinedDate
                }).ToList()
            };
        }

        /// <summary>
        /// Creates a new Customer.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CreateCustomerDto dto)
        {
            var c = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                NationalId = dto.NationalId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(c);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = c.Id }, new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                NationalId = c.NationalId,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            });
        }
    }
}
