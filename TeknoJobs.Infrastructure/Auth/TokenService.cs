﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeknoJobs.Application.Interfaces;
using TeknoJobs.Models;

namespace TeknoJobs.Infrastructure.Auth
{
    /// <summary>
    /// Provides functionality for generating JSON Web Tokens (JWT) for authenticated users.
    /// </summary>
    /// <remarks>This service is designed to create JWT tokens containing user-specific claims, such as user
    /// ID, username, and role. The generated tokens are signed using the HMAC-SHA256 algorithm and include an
    /// expiration time.</remarks>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwt;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwt = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        //function used to generate JWT tokens for authenticated users
        public string GenerateToken(string userId, string username, string role)
        {
            //can be decorated with actual user data from ApplicationUsers and their roles 
            // Claims representing the user's identity in the JWT
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Name, username),
            new Claim("role", role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiresInMinutes), // we have set the token exp. to 20 mins
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
