using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public class DynamicAccessTokenAuthenticationOptions
    {
        public Func<HttpContext, string> SchemeSelector { get; set; } = IntrospectionSelector.Func;
        
        public string DefaultScheme { get; set; } = "Bearer";
    }
}