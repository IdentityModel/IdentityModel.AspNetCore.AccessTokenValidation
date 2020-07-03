using System;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public class DynamicAuthenticationHandlerOptions
    {
        public Func<HttpContext, string> SchemeSelector { get; set; }

        public string DefaultScheme { get; set; } = DynamicAuthenticationHandlerDefaults.NopScheme;
    }
}