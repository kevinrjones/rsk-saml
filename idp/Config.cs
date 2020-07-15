using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using IdentityServer4.Saml;
using IdentityServer4.Saml.Models;

namespace idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api1", "My API #1")
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())},

                    AllowedScopes = {"api1"}
                },

                // MVC client using code flow + pkce
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},

                    RedirectUris = {"http://localhost:5003/signin-oidc"},
                    FrontChannelLogoutUri = "http://localhost:5003/signout-oidc",
                    PostLogoutRedirectUris = {"http://localhost:5003/signout-callback-oidc"},

                    AllowOfflineAccess = true,
                    AllowedScopes = {"openid", "profile", "api1"}
                },

                // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:5002/index.html",
                        "http://localhost:5002/callback.html",
                        "http://localhost:5002/silent.html",
                        "http://localhost:5002/popup.html",
                    },

                    PostLogoutRedirectUris = {"http://localhost:5002/index.html"},
                    AllowedCorsOrigins = {"http://localhost:5002"},

                    AllowedScopes = {"openid", "profile", "api1"}
                },
                new Client
                {
                    ClientId = "http://localhost:5001/saml",
                    ClientName = "RSK SAML2P Test Client",
                    ProtocolType = IdentityServerConstants.ProtocolTypes.Saml2p,
                    AllowedScopes = {"openid", "profile"}
                },
                new Client
                {
                    ClientId = "http://localhost:5002/saml",
                    ClientName = "RSK SAML2P Test Client - Multiple SP",
                    ProtocolType = IdentityServerConstants.ProtocolTypes.Saml2p,
                    AllowedScopes = {"openid", "profile"}
                }
            };

        public static IEnumerable<ServiceProvider> ServiceProviders =>
        
            new[]
            {
                new ServiceProvider
                {
                    EntityId = "http://localhost:5001/saml",
                    AssertionConsumerServices =
                        {new Service(SamlConstants.BindingTypes.HttpPost, "http://localhost:5001/signin-saml")},
                    SigningCertificates = {new X509Certificate2("testclient.cer")}
                },
                new ServiceProvider
                {
                    EntityId = "http://localhost:5002/saml",
                    AssertionConsumerServices =
                        {new Service(SamlConstants.BindingTypes.HttpPost, "http://localhost:5002/signin-saml-1")}
                }
            };
        
    }
}