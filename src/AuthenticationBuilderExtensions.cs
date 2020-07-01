using System;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityModelAspNetCoreAccessTokenValidationAuthenticationBuilderExtensions
    {
        public static void AddAccessToken(this AuthenticationBuilder builder, string scheme, Action<AccessTokenAuthenticationOptions> options)
        {
            builder.AddPolicyScheme(scheme, scheme, policySchemeOptions =>
            {
                AccessTokenAuthenticationOptions atOptions = new AccessTokenAuthenticationOptions();
                options(atOptions);
                
                policySchemeOptions.ForwardDefaultSelector = context =>
                {
                    return atOptions.SchemeSelector(context);
                };
            });
        }
    }
}