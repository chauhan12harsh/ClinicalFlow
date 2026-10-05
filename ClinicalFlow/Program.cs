using ClinicalFlow.Configuration;
using ClinicalFlow.Data;
using ClinicalFlow.Interfaces;
using ClinicalFlow.Middleware;
using ClinicalFlow.Models;
using ClinicalFlow.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register Json to string conversion
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

// Register Services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IEncounterService, EncounterService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

// Bind JWT settings once
var jwtSettings = builder.Configuration.GetSection("Jwt")
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT settings are missing.");

var secretKey = jwtSettings.SecretKey;

if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
{
    throw new InvalidOperationException(
        "JWT secret key must be configured and at least 32 characters long.");
}

// Register the same settings for AuthService through IOptions
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();

    var passwordHasher = services.GetRequiredService<IPasswordHasher<ApplicationUser>>();

    var email = builder.Configuration["DevelopmentSeed:Email"];
    var password = builder.Configuration["DevelopmentSeed:Password"];
    var fullName = builder.Configuration["DevelopmentSeed:FullName"];
    var role = builder.Configuration["DevelopmentSeed:Role"];

    if (!string.IsNullOrWhiteSpace(email) &&
        !string.IsNullOrWhiteSpace(password) &&
        !string.IsNullOrWhiteSpace(fullName) &&
        !string.IsNullOrWhiteSpace(role))
    {
        email = email.Trim().ToLowerInvariant();

        var existingUser = await context.ApplicationUsers
            .AnyAsync(u => u.Email == email);

        if (!existingUser)
        {
            var user = new ApplicationUser
            {
                Email = email,
                FullName = fullName.Trim(),
                Role = role.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = passwordHasher.HashPassword(
                user,
                password);

            context.ApplicationUsers.Add(user);

            await context.SaveChangesAsync();
        }
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
