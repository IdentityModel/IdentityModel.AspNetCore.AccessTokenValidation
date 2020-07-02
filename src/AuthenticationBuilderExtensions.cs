using System;
using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityModelAspNetCoreAccessTokenValidationAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAccessToken(this AuthenticationBuilder builder, string scheme, Action<DynamicAccessTokenAuthenticationOptions> options = null)
        {
            builder.AddScheme<NopAuthenticationOptions, NopAuthenticationHandler>(DynamicAuthenticationDefaults.NopScheme, o => { });

            builder.AddPolicyScheme(scheme, scheme, policySchemeOptions =>
            {
                var atOptions = new DynamicAccessTokenAuthenticationOptions();
                options?.Invoke(atOptions);

                policySchemeOptions.ForwardDefaultSelector = context =>
                    atOptions?.SchemeSelector(context) ?? atOptions.DefaultScheme;
            });

            return builder;
        }
    }
}