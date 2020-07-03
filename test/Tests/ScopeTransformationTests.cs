using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using IdentityModel;
using IdentityModel.AspNetCore.AccessTokenValidation;
using Xunit;

namespace Tests
{
    public class ScopeTransformationTests
    {
        [Fact]
        public void principal_with_single_identity_with_no_scopes_should_be_unchanged()
        {
            var identity = Identity.Create("test",
                new Claim("foo1", "bar1", ClaimValueTypes.String, "test"),
                new Claim("foo2", "2", ClaimValueTypes.Integer, "test"));
            
            var principal = new ClaimsPrincipal(identity);

            var transform = ScopeConverter.SplitScopeClaims(principal);

            transform.Identities.Count().Should().Be(1);
            transform.Identities.First().Claims.Count().Should().Be(2);
            transform.Identities.First().NameClaimType.Should().Be("name");
            transform.Identities.First().RoleClaimType.Should().Be("role");

            var foo1 = transform.Identities.First().FindFirst("foo1");
            foo1.Should().NotBeNull();
            foo1.Value.Should().Be("bar1");
            foo1.ValueType.Should().Be(ClaimValueTypes.String);
            foo1.Issuer.Should().Be("test");

            var foo2 = transform.Identities.First().FindFirst("foo2");
            foo2.Should().NotBeNull();
            foo2.Value.Should().Be("2");
            foo2.ValueType.Should().Be(ClaimValueTypes.Integer);
            foo2.Issuer.Should().Be("test");
        }
        
        [Fact]
        public void principal_multiple_identity_with_no_scopes_should_be_unchanged()
        {
            var identity1 = Identity.Create("test",
                new Claim("foo1", "bar1", ClaimValueTypes.String, "test"),
                new Claim("foo2", "2", ClaimValueTypes.Integer, "test"));
            var identity2 = Identity.Create("test",
                new Claim("foo1", "bar1", ClaimValueTypes.String, "test"),
                new Claim("foo2", "2", ClaimValueTypes.Integer, "test"));
            
            var principal = new ClaimsPrincipal(new[] { identity1, identity2 });

            var transform = ScopeConverter.SplitScopeClaims(principal);

            transform.Identities.Count().Should().Be(2);
            transform.Identities.First().Claims.Count().Should().Be(2);
            transform.Identities.First().NameClaimType.Should().Be("name");
            transform.Identities.First().RoleClaimType.Should().Be("role");
            
            transform.Identities.Last().Claims.Count().Should().Be(2);
            transform.Identities.Last().NameClaimType.Should().Be("name");
            transform.Identities.Last().RoleClaimType.Should().Be("role");

            var foo1 = transform.Identities.First().FindFirst("foo1");
            foo1.Should().NotBeNull();
            foo1.Value.Should().Be("bar1");
            foo1.ValueType.Should().Be(ClaimValueTypes.String);
            foo1.Issuer.Should().Be("test");

            var foo2 = transform.Identities.First().FindFirst("foo2");
            foo2.Should().NotBeNull();
            foo2.Value.Should().Be("2");
            foo2.ValueType.Should().Be(ClaimValueTypes.Integer);
            foo2.Issuer.Should().Be("test");
            
            foo1 = transform.Identities.Last().FindFirst("foo1");
            foo1.Should().NotBeNull();
            foo1.Value.Should().Be("bar1");
            foo1.ValueType.Should().Be(ClaimValueTypes.String);
            foo1.Issuer.Should().Be("test");

            foo2 = transform.Identities.Last().FindFirst("foo2");
            foo2.Should().NotBeNull();
            foo2.Value.Should().Be("2");
            foo2.ValueType.Should().Be(ClaimValueTypes.Integer);
            foo2.Issuer.Should().Be("test");
        }

        [Fact]
        public void principal_single_identity_with_scopes_array_should_be_unchanged()
        {
            var identity = Identity.Create("test",
                new Claim("sub", "123"),
                new Claim("scope", "scope1"),
                new Claim("scope", "scope2"));
            
            var principal = new ClaimsPrincipal(identity);
            
            var transform = ScopeConverter.SplitScopeClaims(principal);

            transform.Identities.Count().Should().Be(1);
            transform.Identities.First().Claims.Count().Should().Be(3);

            transform.Identities.First().FindFirst("sub").Value.Should().Be("123");
            
            var scopes = transform.Identities.First().FindAll("scope").ToList();
            scopes.Count().Should().Be(2);
            scopes.First().Value.Should().Be("scope1");
            scopes.Last().Value.Should().Be("scope2");
        }
        
        [Fact]
        public void principal_single_identity_with_scopes_string_should_be_transformed()
        {
            var identity = Identity.Create("test",
                new Claim("sub", "123"),
                new Claim("scope", "scope1 scope2"));

            var principal = new ClaimsPrincipal(identity);
            
            var transform = ScopeConverter.SplitScopeClaims(principal);

            transform.Identities.Count().Should().Be(1);
            transform.Identities.First().Claims.Count().Should().Be(3);

            transform.Identities.First().FindFirst("sub").Value.Should().Be("123");
            
            var scopes = transform.Identities.First().FindAll("scope").ToList();
            scopes.Count().Should().Be(2);
            scopes.First().Value.Should().Be("scope1");
            scopes.Last().Value.Should().Be("scope2");
        }
    }
}