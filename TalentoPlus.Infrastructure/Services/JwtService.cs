using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Application.DTOs;

namespace TalentoPlus.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration config) => _config = config;

        public LoginResponseDto GenerateToken(string userId, string userName, IList<string> roles)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim("name", userName)
            };
            foreach(var r in roles) claims.Add(new Claim(ClaimTypes.Role, r));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddHours(6);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new LoginResponseDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = expires };
        }
    }
}