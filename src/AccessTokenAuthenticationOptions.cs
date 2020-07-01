using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public class AccessTokenAuthenticationOptions
    {
        public Func<HttpContext, string> SchemeSelector => null;
    }
}