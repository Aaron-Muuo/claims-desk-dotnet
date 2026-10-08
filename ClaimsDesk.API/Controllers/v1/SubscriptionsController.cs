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
    public class SubscriptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubscriptionsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all Subscriptions.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetSubscriptions()
        {
            var data = await _context.Subscriptions
                .Select(s => new SubscriptionDto
                {
                    Id = s.Id,
                    PlanRef = s.PlanRef,
                    PlanName = s.PlanName,
                    Status = s.Status,
                    OrganizationId = s.OrganizationId,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToListAsync();
            return Ok(data);
        }

        /// <summary>
        /// Retrieves a specific Subscription by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDto>> GetSubscription(int id)
        {
            var s = await _context.Subscriptions.FindAsync(id);
            if (s == null) return NotFound();

            return new SubscriptionDto
            {
                Id = s.Id,
                PlanRef = s.PlanRef,
                PlanName = s.PlanName,
                Status = s.Status,
                OrganizationId = s.OrganizationId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            };
        }

        /// <summary>
        /// Creates a new Subscription.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<SubscriptionDto>> PostSubscription(CreateSubscriptionDto dto)
        {
            var s = new Subscription
            {
                PlanRef = dto.PlanRef,
                PlanName = dto.PlanName,
                Status = dto.Status,
                OrganizationId = dto.OrganizationId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Subscriptions.Add(s);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubscription), new { id = s.Id }, new SubscriptionDto
            {
                Id = s.Id,
                PlanRef = s.PlanRef,
                PlanName = s.PlanName,
                Status = s.Status,
                OrganizationId = s.OrganizationId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            });
        }

        /// <summary>
        /// Updates an existing Subscription.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(int id, UpdateSubscriptionDto dto)
        {
            var s = await _context.Subscriptions.FindAsync(id);
            if (s == null) return NotFound();

            s.PlanRef = dto.PlanRef;
            s.PlanName = dto.PlanName;
            s.Status = dto.Status;
            s.OrganizationId = dto.OrganizationId;
            s.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific Subscription.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var s = await _context.Subscriptions.FindAsync(id);
            if (s == null) return NotFound();

            _context.Subscriptions.Remove(s);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
