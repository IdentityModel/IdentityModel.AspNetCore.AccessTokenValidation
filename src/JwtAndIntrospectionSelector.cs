using System;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public static class JwtAndIntrospectionSelector
    {
        public static string Func(HttpContext context)
        {
            var (scheme, token) = DynamicAuthenticationHandler.GetSchemeAndToken(context);

            if (!string.Equals(scheme, "Bearer", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            
            if (token.Contains("."))
            {
                return DynamicAuthenticationHandlerDefaults.JwtBearerDefaultScheme;
            }
            else
            {
                return DynamicAuthenticationHandlerDefaults.IntrospectionDefaultScheme;
            }
        }
    }
}