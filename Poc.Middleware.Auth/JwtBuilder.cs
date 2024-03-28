﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Poc.Middleware.Auth
{
    public class JwtBuilder : IJwtBuilder
    {
        private readonly JwtOptions _options;

        public JwtBuilder(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string GetToken(string userId)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim("userId", userId)
        };
            var expirationDate = DateTime.Now.AddMinutes(_options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: expirationDate);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public string ValidateToken(string token)
        {
            var principal = GetPrincipal(token);
            
            if (principal is null)
                return string.Empty;

            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return string.Empty;
            }
            var userIdClaim = identity?.FindFirst("userId");
            if (userIdClaim == null)
            {
                return string.Empty;
            }
            var userId = userIdClaim.Value;
            return userId;
        }

        private ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                {
                    return null;
                }
                var key = Encoding.UTF8.GetBytes(_options.Secret);
                var parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                IdentityModelEventSource.ShowPII = true;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out _);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
