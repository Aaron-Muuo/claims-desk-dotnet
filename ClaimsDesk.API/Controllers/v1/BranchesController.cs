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
    public class BranchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BranchesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lists all branches across the entire platform.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetBranches()
        {
            var branches = await _context.Branches
                .Select(b => new BranchDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Code = b.Code,
                    IsHeadOffice = b.IsHeadOffice,
                    Address = b.Address,
                    Status = b.Status,
                    OrganizationId = b.OrganizationId
                })
                .ToListAsync();

            return Ok(branches);
        }

        /// <summary>
        /// Retrieves a specific Branch by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDto>> GetBranch(int id)
        {
            var b = await _context.Branches.FindAsync(id);

            if (b == null)
            {
                return NotFound();
            }

            return new BranchDto
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code,
                IsHeadOffice = b.IsHeadOffice,
                Address = b.Address,
                Status = b.Status,
                OrganizationId = b.OrganizationId
            };
        }

        /// <summary>
        /// Creates a new Branch.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BranchDto>> PostBranch(CreateBranchDto dto)
        {
            var branch = new Branch
            {
                Name = dto.Name,
                Code = dto.Code,
                IsHeadOffice = dto.IsHeadOffice,
                Address = dto.Address,
                Status = dto.Status,
                OrganizationId = dto.OrganizationId
            };

            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            var returnDto = new BranchDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Code = branch.Code,
                IsHeadOffice = branch.IsHeadOffice,
                Address = branch.Address,
                Status = branch.Status,
                OrganizationId = branch.OrganizationId
            };

            return CreatedAtAction(nameof(GetBranch), new { id = branch.Id }, returnDto);
        }

 
        /// <summary>
        /// Updates an existing Branch.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranch(int id, UpdateBranchDto dto)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            branch.Name = dto.Name;
            branch.Code = dto.Code;
            branch.IsHeadOffice = dto.IsHeadOffice;
            branch.Address = dto.Address;
            branch.Status = dto.Status;
            branch.OrganizationId = dto.OrganizationId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific Branch.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BranchExists(int id)
        {
            return _context.Branches.Any(e => e.Id == id);
        }
    }
}
