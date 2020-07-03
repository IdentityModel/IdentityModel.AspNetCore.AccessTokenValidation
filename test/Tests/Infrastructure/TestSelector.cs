using System.Net.Http;
using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Http;

namespace Tests.Infrastructure
{
    public static class TestSelector
    {
        public static string Func(HttpContext context)
        {
            var (scheme, token) = DynamicAuthenticationHandler.GetSchemeAndToken(context);

            if (scheme is null) return null;
            
            if (scheme == "unknown")
            {
                return null;
            }

            return "test";
        }
    }
}