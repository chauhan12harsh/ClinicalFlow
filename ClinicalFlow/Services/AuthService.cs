using ClinicalFlow.Configuration;
using ClinicalFlow.Data;
using ClinicalFlow.Dtos.Auth;
using ClinicalFlow.Interfaces;
using ClinicalFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicalFlow.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;

        public AuthService(
            ApplicationDbContext context,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IOptions<JwtSettings> jwtOptions,
            IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtOptions.Value;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _context.ApplicationUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                return null;

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
                return null;

            var secretKey = _configuration["Jwt:SecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("JWT signing key is not configured.");
            }

            var expiresAt = DateTime.UtcNow.AddMinutes( _jwtSettings.ExpirationMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.ApplicationUserId.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),                
                new(ClaimTypes.Role, user.Role.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                ExpiresAtUtc = expiresAt
            };
        }
    }
}
