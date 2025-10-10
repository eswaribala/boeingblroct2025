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
    public sealed class LocalJwtIssuer : IJwtIssuer
    {
        private readonly JwtOptions _opts;
        private readonly SigningCredentials _creds;

        public LocalJwtIssuer(JwtOptions opts)
        {
            _opts = opts;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opts.Secret));
            _creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public string Issue(string subject, IEnumerable<string>? roles = null, IEnumerable<string>? scopes = null, TimeSpan? lifetime = null)
        {
            roles ??= (_opts.DefaultRoles ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var scopeStr = string.Join(' ', scopes ?? (_opts.DefaultScopes ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

            var now = DateTime.UtcNow;
            var claims = new List<Claim> { new("sub", subject) };
            if (!string.IsNullOrWhiteSpace(scopeStr)) claims.Add(new("scp", scopeStr));
            foreach (var r in roles) claims.Add(new(ClaimTypes.Role, r));

            var jwt = new JwtSecurityToken(
                issuer: _opts.Issuer,
                audience: _opts.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(lifetime ?? TimeSpan.FromMinutes(30)),
                signingCredentials: _creds
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
