using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public static class ScopeClaimsTransformerExtension
    {
        public static IServiceCollection AddScopeTransformation(this IServiceCollection services)
        {
            return services.AddSingleton<IClaimsTransformation, ScopeClaimsTransformer>();
        }
    }
    
    public class ScopeClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult(ScopeConverter.SplitScopeClaims(principal));
        }
    }
}