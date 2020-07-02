using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public class AccessTokenAuthenticationOptions
    {
        public static string DefaultSelector(HttpContext context, string JwtHandlerScheme,
            string IntrospectionHandlerScheme)
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
                    return JwtHandlerScheme;
                }
                else
                {
                    return IntrospectionHandlerScheme;
                }
            }

            return null;
        }

        public Func<HttpContext, string> SchemeSelector { get; set; } = context =>
            DefaultSelector(context, DefaultJwtHandlerScheme, DefaultIntrospectionHandlerScheme);
        
        public const string DefaultJwtHandlerScheme = "Bearer";
        public const string DefaultIntrospectionHandlerScheme = "Introspection";
    }
}