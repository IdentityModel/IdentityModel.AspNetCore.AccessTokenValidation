using System;
using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityModelAspNetCoreAccessTokenValidationAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddDynamicAuthenticationHandler(this AuthenticationBuilder builder, string scheme, Action<DynamicAuthenticationHandlerOptions> options = null)
        {
            builder.AddScheme<NopAuthenticationOptions, NopAuthenticationHandler>(DynamicAuthenticationHandlerDefaults.NopScheme, o => { });

            builder.AddPolicyScheme(scheme, scheme, policySchemeOptions =>
            {
                var atOptions = new DynamicAuthenticationHandlerOptions();
                options?.Invoke(atOptions);

                policySchemeOptions.ForwardDefaultSelector = context =>
                    atOptions?.SchemeSelector(context) ?? atOptions.DefaultScheme;
            });

            return builder;
        }
    }
}