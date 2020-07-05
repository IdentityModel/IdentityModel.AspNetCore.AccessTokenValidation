using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public static class ReferenceToken
    {
        public static Func<HttpContext, string> Forward(string introspectionScheme = "Introspection")
        {
            string Select(HttpContext context)
            {
                var (scheme, credential) = GetSchemeAndCredential(context);
                if (scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) &&
                    !credential.Contains("."))
                {
                    return introspectionScheme;
                }

                return null;
            }

            return Select;
        }
        
        
        public static (string, string) GetSchemeAndCredential(HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(header))
            {
                return ("", "");
            }

            var parts = header.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return ("", "");
            }

            return (parts[0], parts[1]);
        }
    }
}