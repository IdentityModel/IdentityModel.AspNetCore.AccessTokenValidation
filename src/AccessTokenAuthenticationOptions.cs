using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public class AccessTokenAuthenticationOptions
    {
        public string DefaultSelector(HttpContext context, string jwtHandlerScheme,
            string introspectionHandlerScheme)
        {
            var prefix = "Bearer";
            var header = context.Request.Headers["Authorization"].FirstOrDefault();
                
            if (string.IsNullOrEmpty(header))
            {
                return null;
            }

            if (header.StartsWith(prefix + " ", StringComparison.OrdinalIgnoreCase))
            {
                var token = header.Substring(prefix.Length + 1).Trim();

                if (token.Contains("."))
                {
                    return jwtHandlerScheme;
                }
                else
                {
                    return introspectionHandlerScheme;
                }
            }

            return null;
        }

        public Func<HttpContext, string> SchemeSelector { get; set; }
        
        public string DefaultJwtHandlerScheme { get; set; } = "Bearer";
        public string DefaultIntrospectionHandlerScheme { get; set; } = "Introspection";
    }
}