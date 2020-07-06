using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    /// <summary>
    /// Extension methods for scope claims transformation
    /// </summary>
    public static class ScopeClaimsTransformerExtension
    {
        /// <summary>
        /// Adds an IClaimsTransformation for transforming scope claims to the DI container.
        /// </summary>
        public static IServiceCollection AddScopeTransformation(this IServiceCollection services)
        {
            return services.AddSingleton<IClaimsTransformation, ScopeClaimsTransformer>();
        }
    }
    
    /// <summary>
    /// Claims transformer to transform scope claims from space separated to separate claims
    /// </summary>
    public class ScopeClaimsTransformer : IClaimsTransformation
    {
        /// <inheritdoc />
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult(principal.NormalizeScopeClaims());
        }
    }
}