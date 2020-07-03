using System;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public class DynamicAuthenticationHandlerOptions
    {
        public Func<HttpContext, string> SchemeSelector { get; set; } = JwtAndIntrospectionSelector.Func;

        public string DefaultScheme { get; set; } = DynamicAuthenticationHandlerDefaults.NopScheme;
    }
}