using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public static class DynamicAuthenticationHandler
    {
        public static (string, string) GetSchemeAndToken(HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(header))
            {
                return (null, null);
            }

            var parts = header.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return (null, null);
            }

            return (parts[0], parts[1]);
        }
    }
}