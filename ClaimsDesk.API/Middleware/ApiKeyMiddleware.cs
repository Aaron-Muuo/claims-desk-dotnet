using ClaimsDesk.API.Data;
using ClaimsDesk.API.Helpers;
using ClaimsDesk.API.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsDesk.API.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string AppIdHeader = "X-App-Id";
        private const string ApiKeyHeader = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            // Skip API Key check for swagger, root, static files
            var path = context.Request.Path.Value;
            if (string.IsNullOrEmpty(path) || path == "/" || path.StartsWith("/swagger") || path.StartsWith("/Images"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(AppIdHeader, out var extractedAppId) ||
                !context.Request.Headers.TryGetValue(ApiKeyHeader, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key and App ID are required.");
                return;
            }

            var appId = extractedAppId.ToString();
            var apiKey = extractedApiKey.ToString();

            // Look up the record
            var record = await dbContext.ApiKeys
                .Include(k => k.Organization)
                .FirstOrDefaultAsync(k => k.AppId == appId && k.IsActive);

            if (record == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid App ID.");
                return;
            }

            if (record.ExpiresAt.HasValue && record.ExpiresAt.Value <= DateTime.UtcNow)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key has expired.");
                return;
            }

            // Verify hash
            var incomingHash = ApiKeyGenerator.ComputeSha256Hash(apiKey);
            if (!string.Equals(record.KeyHash, incomingHash, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key.");
                return;
            }

            // Check record Type
            if (record.Type == ApiKeyType.Platform)
            {
                // Official ClaimsDesk client. Tenant scoping deferred to JWT.
                context.Items["IsPlatformClient"] = true;
            }
            else if (record.Type == ApiKeyType.TenantIntegration)
            {
                // Machine-to-machine developer call
                if (record.OrganizationId == null)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Tenant Integration API Key is missing OrganizationId.");
                    return;
                }

                // Force tenant context
                context.Items["TenantId"] = record.OrganizationId;
            }

            await _next(context);
        }
    }
}
