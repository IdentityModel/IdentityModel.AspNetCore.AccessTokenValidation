using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.AspNetCore.AccessTokenValidation;
using Tests.Infrastructure;
using Xunit;

namespace Tests
{
    public class DispatchingTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task default_config_should_throw(bool sendToken)
        {
            var server = new Server();
            var client = server.CreateClient();
            
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api");

            if (sendToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("some", "token");
            }

            Func<Task> act = async () => { await client.SendAsync(request); };
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task test_selector_no_handler_no_token_should_401()
        {
            var server = new Server
            {
                AccessTokenOptions = o =>
                {
                    o.SchemeSelector = TestSelector.Func;
                }
            };
            
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api");

            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task test_selector_no_handler_unknown_token_should_401()
        {
            var server = new Server
            {
                AccessTokenOptions = o =>
                {
                    o.SchemeSelector = TestSelector.Func;
                }
            };
            
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api");
            request.Headers.Authorization = new AuthenticationHeaderValue("unknown", "token");

            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task test_selector_no_handler_known_token_should_throw()
        {
            var server = new Server
            {
                AccessTokenOptions = o =>
                {
                    o.DefaultScheme = "test";
                    o.SchemeSelector = TestSelector.Func;
                }
            };
            
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api");
            request.Headers.Authorization = new AuthenticationHeaderValue("known", "token");


            Func<Task> act = async () => { await client.SendAsync(request); };
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task test_selector_test_handler_no_token_should_401()
        {
            var server = new Server
            {
                AddTestHandler = true,
                AccessTokenOptions = o =>
                {
                    o.SchemeSelector = TestSelector.Func;
                }
            };
            
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api");

            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task test_selector_test_handler_known_token_should_200()
        {
            var server = new Server
            {
                AddTestHandler = true,
                AccessTokenOptions = o =>
                {
                    o.DefaultScheme = "test";
                    o.SchemeSelector = TestSelector.Func;
                }
            };
            
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api");
            request.Headers.Authorization = new AuthenticationHeaderValue("known", "token");

            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        
        
        [Fact]
        public async Task handlers_and_token_should_200()
        {
            var server = new Server
            {
                AddTestHandler = true, 
                AccessTokenOptions = o => { o.SchemeSelector = context => "test"; }
            };
            
            var client = server.CreateClient();
            
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api");
            request.Headers.Authorization = new AuthenticationHeaderValue("prefix", "token");

            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}