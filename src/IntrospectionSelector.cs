using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public class IntrospectionSelector
    {
        public static string Func(HttpContext context)
        {
            var (scheme, token) = GetSchemeAndToken(context);

            if (!string.Equals(scheme, "Bearer", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            
            if (token.Contains("."))
            {
                return DynamicAuthenticationDefaults.JwtBearerDefaultScheme;
            }
            else
            {
                return DynamicAuthenticationDefaults.IntrospectionDefaultScheme;
            }
        }

        public static (string, string) GetSchemeAndToken(HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(header))
            {
                return (null, null);
            }

            var parts = header.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 1)
            {
                return (null, null);
            }

            return (parts[0], parts[1]);
        }
    }
}