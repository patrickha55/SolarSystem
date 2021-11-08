﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;

namespace SolarSystem.Services.AuthWithJwt
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;

        public AuthManager(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> ValidateUserAsync(SignInDTO request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request),
                    $"Invalid signin attempt. Please check in {nameof(ValidateUserAsync)}");

            _user = await _userManager.FindByEmailAsync(request.Email);

            var validPassword = await _userManager.CheckPasswordAsync(_user, request.Password);

            return (_user is not null && validPassword);
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("KEY");
            if (key == null)
                throw new NullReferenceException(
                    $"Environment variable KEY is null in {nameof(GetSigningCredentials)}.");

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync()
        {
            if (_user is null) throw new NullReferenceException($"User object is null in {nameof(GetClaimsAsync)}.");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            claims.AddRange(
                roles.Select(r => new Claim(ClaimTypes.Role, r))
            );

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            Int32.TryParse(jwtSettings.GetSection("Lifetime").Value, out var minutes);

            var expiration = DateTime.Now.AddMinutes(minutes);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            return token;
        }
    }
}