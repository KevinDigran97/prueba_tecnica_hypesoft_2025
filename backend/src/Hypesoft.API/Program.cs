using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AspNetCoreRateLimit;
using Hypesoft.Application;
using Hypesoft.Infrastructure;
using Hypesoft.Infrastructure.Repositories.InMemory;
using Hypesoft.API.Middlewares;
using System.Security.Claims;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/hypesoft-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add infrastructure services
builder.Services.AddInfrastructure(builder.Configuration, isDevelopment);

// Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hypesoft API",
        Version = "v1",
        Description = "Product Management System API",
        Contact = new OpenApiContact
        {
            Name = "Hypesoft Labs",
            Email = "conta@hypesoft.com"
        }
    });

    // JWT Authentication in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments (optional)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// JWT Authentication with Keycloak
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var keycloakSettings = builder.Configuration.GetSection("Keycloak");
        var authority = keycloakSettings["Authority"];

        if (string.IsNullOrEmpty(authority))
        {
            throw new InvalidOperationException("Keycloak:Authority is not configured in appsettings.json");
        }

        options.Authority = authority;
        options.RequireHttpsMetadata = !isDevelopment; // Set to false in development, true in production

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,
            ValidateAudience = true,
            ValidAudience = "hypesoft-api",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var error = context.Exception;
                Log.Error("Authentication failed: {ErrorType} - {ErrorMessage}", 
                    error.GetType().Name, 
                    error.Message);
                
                // Log detalles adicionales para debugging
                if (error is SecurityTokenInvalidAudienceException audienceError)
                {
                    Log.Error("Invalid audience. Expected: {ExpectedAudience}, Actual: {ActualAudience}", 
                        audienceError.InvalidAudience, 
                        audienceError.InvalidAudience ?? "null");
                }
                else if (error is SecurityTokenInvalidIssuerException issuerError)
                {
                    Log.Error("Invalid issuer. Expected: {ExpectedIssuer}, Actual: {ActualIssuer}", 
                        issuerError.InvalidIssuer, 
                        issuerError.InvalidIssuer ?? "null");
                }
                
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var claims = context.Principal?.Claims;
                var userId = context.Principal?.Identity?.Name ?? "Unknown";
                var audience = claims?.FirstOrDefault(c => c.Type == "aud")?.Value ?? "Not set";
                
                Log.Information("Token validated successfully. User: {User}, Audience: {Audience}", 
                    userId, 
                    audience);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Log.Warning("Authentication challenge. Error: {Error}, ErrorDescription: {ErrorDescription}", 
                    context.Error, 
                    context.ErrorDescription);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Rate Limiting Configuration
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    try
    {
        await Hypesoft.Infrastructure.DependencyInjection.InitializeDatabaseAsync(scope.ServiceProvider, isDevelopment);
        Log.Information("Database initialization completed");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while initializing the database");
        if (!isDevelopment)
        {
            throw;
        }
        // In development, continue with in-memory data
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hypesoft API v1");
        c.RoutePrefix = "swagger";
    });
}

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

// Middlewares
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

app.UseIpRateLimiting();

app.UseCors(); // This will apply the default CORS policy

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

Log.Information("Starting Hypesoft API...");

app.Run();
