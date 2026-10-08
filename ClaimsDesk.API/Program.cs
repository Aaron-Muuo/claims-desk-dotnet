using ClaimsDesk.API.Data;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using ClaimsDesk.API.SwaggerExtensions;

var builder = WebApplication.CreateBuilder(args);

// CLI Command for generating Platform API keys
if (args.Contains("--generate-platform-key"))
{
    var (appId, apiSecret, keyHash, keyPrefix) = ClaimsDesk.API.Helpers.ApiKeyGenerator.GenerateNewKey();
    Console.WriteLine("=== NEW PLATFORM API KEY ===");
    Console.WriteLine($"AppName: ClaimsDesk Platform Client");
    Console.WriteLine($"AppId: {appId}");
    Console.WriteLine($"ApiSecret: {apiSecret}");
    Console.WriteLine($"KeyHash: {keyHash}");
    Console.WriteLine("============================");

    var connectionString1 = builder.Configuration.GetConnectionString("DefaultConnection");
    var optionsBuilder = new DbContextOptionsBuilder<ClaimsDesk.API.Data.AppDbContext>();
    optionsBuilder.UseMySql(connectionString1, ServerVersion.AutoDetect(connectionString1));
    using var db = new ClaimsDesk.API.Data.AppDbContext(optionsBuilder.Options);

    db.ApiKeys.Add(new ClaimsDesk.API.Models.ApiKey 
    {
        AppName = "ClaimsDesk Platform Client",
        AppId = appId,
        KeyHash = keyHash,
        KeyPrefix = keyPrefix,
        Type = ClaimsDesk.API.Models.Enums.ApiKeyType.Platform,
        Platform = ClaimsDesk.API.Models.Enums.ClientPlatform.Web,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    });
    db.SaveChanges();
    Console.WriteLine("Saved to database successfully.");
    return;
}


// Register MySQL DbContext with AutoDetect for MariaDB/MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Add CORS policy for the separate React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// --- ADDED: API Versioning Services ---
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Defaults to v1.0 if not specified
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true; // Adds 'api-supported-versions' header to responses
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Formats version as 'v1', 'v2' in Swagger
    options.SubstituteApiVersionInUrl = true; // Replaces {version:apiVersion} in routes
});

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ClaimsDesk.API.Helpers.IAuditLogger, ClaimsDesk.API.Helpers.AuditLogger>();
builder.Services.AddScoped<ClaimsDesk.API.Helpers.ILicenseKeyService, ClaimsDesk.API.Helpers.LicenseKeyService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>(); // Reads discovered API versions

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

// Custom Landing Page for Root
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;
    
    // Serve HTML landing page at root
    if (path == "/")
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        var html = @"
<!DOCTYPE html>
<html>
<head>
    <title>ClaimsDesk API</title>
    <style>
        body { font-family: Arial, sans-serif; display: flex; flex-direction: column; align-items: center; justify-content: center; height: 100vh; margin: 0; background-color: #f8fafc; }
        .container { text-align: center; background: white; padding: 50px; border-radius: 12px; border: 1px solid #e2e8f0; }
        h1 { color: #1e293b; font-size: 24px; margin-top: 20px; margin-bottom: 10px; }
        p { color: #64748b; margin-bottom: 30px; }
        a.button { display: inline-block; padding: 12px 24px; background-color: #2F6240; color: white; text-decoration: none; border-radius: 6px; font-weight: bold; transition: background-color 0.2s; }
        a.button:hover { background-color: #E3A000; }
        img.logo { width: 150px; height: auto; }
    </style>
</head>
<body>
    <div class='container'>
        <img src='/Images/logo.png' alt='ClaimsDesk Logo' class='logo' />
        <h1>ClaimsDesk API</h1>
        <p>Insurance Operations & Claims Management Engine</p>
        <a href='/swagger' class='button'>Open Swagger Docs</a>
    </div>
</body>
</html>";
        await context.Response.WriteAsync(html);
        return;
    }


    await next();
});

// UseCors MUST be placed before UseSwagger so cross-origin requests to swagger.json get CORS headers
app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var descriptions = app.DescribeApiVersions();
    foreach (var description in descriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            $"ClaimsDesk API {description.GroupName.ToUpperInvariant()}"
        );
    }
});

app.UseHttpsRedirection();

// API Key Authentication Middleware
app.UseMiddleware<ClaimsDesk.API.Middleware.ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

// Run seeder automatically on startup (Safe because it checks if data exists)
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var licenseService = scope.ServiceProvider.GetRequiredService<ClaimsDesk.API.Helpers.ILicenseKeyService>();
await DataSeeder.SeedAllAsync(context, licenseService);

app.Run();
