using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.AccessTokenValidation;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests.Infrastructure
{
    public class Server
    {
        public Action<AccessTokenAuthenticationOptions> AccessTokenOptions { get; set; }
        public bool AddTestHandler { get; set; }
        
        public TestServer CreateServer()
        {
            
            return new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    var builder = services.AddAuthentication("token");
                    
                    builder.AddAccessToken("token", AccessTokenOptions);

                    if (AddTestHandler)
                    {
                        builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("test", o => { });
                    }

                })
                .Configure(app =>
                {
                    app.UseAuthentication();

                    app.Use((context, next) =>
                    {
                        var user = context.User;

                        if (user.Identity.IsAuthenticated)
                        {
                            context.Response.StatusCode = 200;
                        }
                        else
                        {
                            context.Response.StatusCode = 401;
                        }

                        return Task.CompletedTask;
                    });
                }));
        }
        
        public HttpClient CreateClient()
        {
            return CreateServer().CreateClient();
        }
    }
}