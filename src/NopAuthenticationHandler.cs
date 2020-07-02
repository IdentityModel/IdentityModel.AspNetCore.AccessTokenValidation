using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    public class NopAuthenticationOptions : AuthenticationSchemeOptions
    { }
    
    public class NopAuthenticationHandler : AuthenticationHandler<NopAuthenticationOptions>
    {
        public NopAuthenticationHandler(IOptionsMonitor<NopAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}