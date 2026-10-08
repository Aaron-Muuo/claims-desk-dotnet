using ClaimsDesk.API.Data;
using ClaimsDesk.API.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ClaimsDesk.API.Helpers
{
    public interface IAuditLogger
    {
        Task LogAsync(string code, int branchId, int? userId = null, string? rawDescription = null, object? metadata = null);
    }

    public class AuditLogger : IAuditLogger
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogger(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string code, int branchId, int? userId = null, string? rawDescription = null, object? metadata = null)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var routeData = httpContext?.GetRouteData();

            var auditTrail = new AuditTrail
            {
                Code = code,
                RawDescription = rawDescription,
                BranchId = branchId,
                UserId = userId,
                SourceIp = httpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = httpContext?.Request?.Headers.UserAgent.ToString(),
                RequestRoute = httpContext?.Request?.Path.Value,
                SourceController = routeData?.Values["controller"]?.ToString(),
                SourceAction = routeData?.Values["action"]?.ToString(),
                Metadata = metadata != null ? JsonSerializer.Serialize(metadata) : null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AuditTrails.Add(auditTrail);
            await _context.SaveChangesAsync();
        }
    }
}
