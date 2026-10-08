using ClaimsDesk.API.Data;
using ClaimsDesk.API.Helpers;
using ClaimsDesk.API.Models;
using ClaimsDesk.API.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsDesk.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ApiKeysController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApiKeysController(AppDbContext context)
        {
            _context = context;
        }

        public class CreateApiKeyRequest
        {
            public string AppName { get; set; } = string.Empty;
            public int OrganizationId { get; set; }
            public ClientPlatform Platform { get; set; } = ClientPlatform.ServerToDevice;
        }

        public class UpdateApiKeyRequest
        {
            public string AppName { get; set; } = string.Empty;
            public bool IsActive { get; set; }
        }

        /// <summary>
        /// Generates a new Tenant Integration API Key for an organization.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GenerateKey([FromBody] CreateApiKeyRequest request)
        {
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null)
            {
                return NotFound(new { message = "Organization not found." });
            }

            var (appId, apiSecret, keyHash, keyPrefix) = ApiKeyGenerator.GenerateNewKey();

            var apiKey = new ApiKey
            {
                AppName = request.AppName,
                AppId = appId,
                KeyHash = keyHash,
                KeyPrefix = keyPrefix,
                Type = ApiKeyType.TenantIntegration,
                Platform = request.Platform,
                OrganizationId = request.OrganizationId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.ApiKeys.Add(apiKey);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Id = apiKey.Id,
                AppName = apiKey.AppName,
                AppId = apiKey.AppId,
                ApiSecret = apiSecret, // ONLY TIME THIS IS SHOWN
                KeyPrefix = apiKey.KeyPrefix,
                OrganizationId = apiKey.OrganizationId,
                Message = "Store this API Secret securely. It will not be shown again."
            });
        }

        /// <summary>
        /// Retrieves all API keys across the platform.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllApiKeys()
        {
            var keys = await _context.ApiKeys
                .Select(k => new
                {
                    k.Id,
                    k.AppName,
                    k.AppId,
                    k.KeyPrefix,
                    k.Type,
                    k.Platform,
                    k.OrganizationId,
                    k.IsActive,
                    k.CreatedAt,
                    k.ExpiresAt
                })
                .ToListAsync();

            return Ok(keys);
        }

        /// <summary>
        /// Updates an existing API key's name or active status.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApiKey(int id, [FromBody] UpdateApiKeyRequest request)
        {
            var apiKey = await _context.ApiKeys.FindAsync(id);
            if (apiKey == null)
            {
                return NotFound(new { message = "API Key not found." });
            }

            apiKey.AppName = request.AppName;
            apiKey.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes an API key permanently.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApiKey(int id)
        {
            var apiKey = await _context.ApiKeys.FindAsync(id);
            if (apiKey == null)
            {
                return NotFound(new { message = "API Key not found." });
            }

            _context.ApiKeys.Remove(apiKey);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
