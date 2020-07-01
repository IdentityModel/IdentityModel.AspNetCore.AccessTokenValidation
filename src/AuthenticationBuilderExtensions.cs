using System;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityModelAspNetCoreAccessTokenValidationAuthenticationBuilderExtensions
    {
        public static void AddAccessToken(this AuthenticationBuilder builder, string scheme, Action<AccessTokenAuthenticationOptions> options = null)
        {
            builder.AddScheme<NopAuthenticationOptions, NopAuthenticationHandler>("nop", o => { });
            
            builder.AddPolicyScheme(scheme, scheme, policySchemeOptions =>
            {
                var atOptions = new AccessTokenAuthenticationOptions();
                options?.Invoke(atOptions);

                policySchemeOptions.ForwardDefaultSelector = context => atOptions.SchemeSelector(context) ?? "nop";
            });
        }
    }
}