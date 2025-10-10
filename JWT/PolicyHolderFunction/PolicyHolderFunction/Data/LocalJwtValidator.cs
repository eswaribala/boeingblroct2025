using Microsoft.IdentityModel.Tokens;
using PolicyHolderFunction.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Data
{
    public sealed class LocalJwtValidator : IJwtValidator
    {
        private readonly TokenValidationParameters _tvp;
        private readonly JwtSecurityTokenHandler _handler = new();

        public LocalJwtValidator(JwtOptions opts)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opts.Secret));
            _tvp = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = opts.Issuer,
                ValidateAudience = true,
                ValidAudience = opts.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        }

        public ClaimsPrincipal Validate(string token) => _handler.ValidateToken(token, _tvp, out _);
    }
}
