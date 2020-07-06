using System;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure
{
    public class Server
    {
        public string ForwardScheme { get; set; }
        
        public bool AddForwarder { get; set; }
        
        public TestServer CreateServer()
        {
            return new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    var builder = services.AddAuthentication("test");
                    builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("test", o =>
                    {
                        if (AddForwarder)
                        {
                            if (!string.IsNullOrEmpty(ForwardScheme))
                            {
                                o.ForwardDefaultSelector = Selector.ForwardReferenceToken(ForwardScheme);
                            }
                            else
                            {
                                o.ForwardDefaultSelector = Selector.ForwardReferenceToken();
                            }
                        }
                    });
                    
                    

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