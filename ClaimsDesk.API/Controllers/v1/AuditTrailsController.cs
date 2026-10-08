using Asp.Versioning;
using ClaimsDesk.API.Data;
using ClaimsDesk.API.DTOs.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimsDesk.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuditTrailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuditTrailsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all AuditTrails.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditTrailDto>>> GetAuditTrails([FromQuery] int? branchId)
        {
            var query = _context.AuditTrails.AsQueryable();

            if (branchId.HasValue)
            {
                query = query.Where(a => a.BranchId == branchId.Value);
            }

            var auditTrails = await query
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AuditTrailDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    SourceIp = a.SourceIp,
                    UserAgent = a.UserAgent,
                    RawDescription = a.RawDescription,
                    RequestRoute = a.RequestRoute,
                    SourceController = a.SourceController,
                    SourceAction = a.SourceAction,
                    Metadata = a.Metadata,
                    BranchId = a.BranchId,
                    UserId = a.UserId,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(auditTrails);
        }

        /// <summary>
        /// Retrieves a specific AuditTrail by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuditTrailDto>> GetAuditTrail(int id)
        {
            var a = await _context.AuditTrails.FindAsync(id);

            if (a == null)
            {
                return NotFound();
            }

            return new AuditTrailDto
            {
                Id = a.Id,
                Code = a.Code,
                SourceIp = a.SourceIp,
                UserAgent = a.UserAgent,
                RawDescription = a.RawDescription,
                RequestRoute = a.RequestRoute,
                SourceController = a.SourceController,
                SourceAction = a.SourceAction,
                Metadata = a.Metadata,
                BranchId = a.BranchId,
                UserId = a.UserId,
                CreatedAt = a.CreatedAt
            };
        }
    }
}
