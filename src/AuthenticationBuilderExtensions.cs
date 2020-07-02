using System;
using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityModelAspNetCoreAccessTokenValidationAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAccessToken(this AuthenticationBuilder builder, string scheme, Action<AccessTokenAuthenticationOptions> options = null)
        {
            builder.AddScheme<NopAuthenticationOptions, NopAuthenticationHandler>("nop", o => { });
            
            builder.AddPolicyScheme(scheme, scheme, policySchemeOptions =>
            {
                var atOptions = new AccessTokenAuthenticationOptions();
                options?.Invoke(atOptions);

                if (atOptions.SchemeSelector is null)
                {
                    policySchemeOptions.ForwardDefaultSelector = context => 
                        atOptions.DefaultSelector(context, atOptions.DefaultJwtHandlerScheme, atOptions.DefaultIntrospectionHandlerScheme) ?? "nop";
                }
                else
                {
                    policySchemeOptions.ForwardDefaultSelector = context => 
                        atOptions.SchemeSelector(context) ?? "nop";
                }
            });

            return builder;
        }
    }
}