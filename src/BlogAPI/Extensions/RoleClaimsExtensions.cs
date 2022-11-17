using BlogAPI.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace BlogAPI.Extensions
{
    public static class RoleClaimsExtensions
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
            };

            claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));

            return claims;
        }
    }
}
