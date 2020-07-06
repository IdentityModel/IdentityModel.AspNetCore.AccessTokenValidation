using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityModel.AspNetCore.AccessTokenValidation
{
    /// <summary>
    /// Logic for normalizing scope claims to separate claim types
    /// </summary>
    public static class ScopeConverter
    {
        /// <summary>
        /// Logic for normalizing scope claims to separate claim types
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static ClaimsPrincipal NormalizeScopeClaims(this ClaimsPrincipal principal)
        {
            var identities = new List<ClaimsIdentity>();

            foreach (var id in principal.Identities)
            {
                var identity = new ClaimsIdentity(id.AuthenticationType, id.NameClaimType, id.RoleClaimType);

                foreach (var claim in id.Claims)
                {
                    if (claim.Type == "scope")
                    {
                        if (claim.Value.Contains(' '))
                        {
                            var scopes = claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                            foreach (var scope in scopes)
                            {
                                identity.AddClaim(new Claim("scope", scope, claim.ValueType, claim.Issuer));
                            }
                        }
                        else
                        {
                            identity.AddClaim(claim);
                        }
                    }
                    else
                    {
                        identity.AddClaim(claim);
                    }
                }
                
                identities.Add(identity);
            }
            
            return new ClaimsPrincipal(identities);
        }
    }
}