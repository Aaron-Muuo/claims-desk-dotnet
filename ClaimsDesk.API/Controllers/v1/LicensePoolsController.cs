using Asp.Versioning;
using ClaimsDesk.API.Data;
using ClaimsDesk.API.DTOs.v1;
using ClaimsDesk.API.Models;
using ClaimsDesk.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimsDesk.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LicensePoolsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILicenseKeyService _licenseKeyService;

        public LicensePoolsController(AppDbContext context, ILicenseKeyService licenseKeyService)
        {
            _context = context;
            _licenseKeyService = licenseKeyService;
        }

        /// <summary>
        /// Retrieves a list of all LicensePools.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LicensePoolDto>>> GetLicensePools()
        {
            var data = await _context.LicensePools
                .Select(l => new LicensePoolDto
                {
                    Id = l.Id,
                    LicenseType = l.LicenseType,
                    LicenseKey = l.LicenseKey,
                    Seats = l.Seats,
                    Expires = l.Expires,
                    ExpiresAt = l.ExpiresAt,
                    SubscriptionId = l.SubscriptionId
                }).ToListAsync();
            return Ok(data);
        }

        /// <summary>
        /// Retrieves a specific LicensePool by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LicensePoolDto>> GetLicensePool(int id)
        {
            var l = await _context.LicensePools.FindAsync(id);
            if (l == null) return NotFound();

            return new LicensePoolDto
            {
                Id = l.Id,
                LicenseType = l.LicenseType,
                LicenseKey = l.LicenseKey,
                Seats = l.Seats,
                Expires = l.Expires,
                ExpiresAt = l.ExpiresAt,
                SubscriptionId = l.SubscriptionId
            };
        }

        /// <summary>
        /// Creates a new LicensePool.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LicensePoolDto>> PostLicensePool(CreateLicensePoolDto dto)
        {
            // Auto-generate secure license key using the new helper
            string key = _licenseKeyService.Generate(dto.LicenseType, dto.Seats, dto.Expires, dto.ExpiresAt);

            var l = new LicensePool
            {
                LicenseType = dto.LicenseType,
                LicenseKey = key,
                Seats = dto.Seats,
                Expires = dto.Expires,
                ExpiresAt = dto.ExpiresAt,
                SubscriptionId = dto.SubscriptionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.LicensePools.Add(l);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLicensePool), new { id = l.Id }, new LicensePoolDto
            {
                Id = l.Id,
                LicenseType = l.LicenseType,
                LicenseKey = l.LicenseKey,
                Seats = l.Seats,
                Expires = l.Expires,
                ExpiresAt = l.ExpiresAt,
                SubscriptionId = l.SubscriptionId
            });
        }

        /// <summary>
        /// Updates an existing LicensePool.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLicensePool(int id, UpdateLicensePoolDto dto)
        {
            var l = await _context.LicensePools.FindAsync(id);
            if (l == null) return NotFound();

            l.LicenseType = dto.LicenseType;
            l.Seats = dto.Seats;
            l.Expires = dto.Expires;
            l.ExpiresAt = dto.ExpiresAt;
            l.SubscriptionId = dto.SubscriptionId;
            l.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific LicensePool.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicensePool(int id)
        {
            var l = await _context.LicensePools.FindAsync(id);
            if (l == null) return NotFound();

            _context.LicensePools.Remove(l);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
