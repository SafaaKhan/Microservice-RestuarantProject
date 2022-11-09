using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Services.Identity
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        //Identity resource: data you want to secure them identity data and api

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };// you can add cutomized identity resources

        public static IEnumerable<ApiScope> ApiScopes =>
           new List<ApiScope>
           {
               new ApiScope("mangoAdmin","Mango Server"),
               new ApiScope(name: "read",displayName: "Read your data"),
               new ApiScope(name: "write",displayName: "Write your data"),
               new ApiScope(name: "delete",displayName: "Delete your data"),
           };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client //general client
                {
                    ClientId="client",
                    ClientSecrets={new Secret("secret".Sha256())},//in production, you will have a secret key
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={"read","write","profile"} //profile: build in scope
                },
                 new Client //web(mango) client
                {
                    ClientId="mango",
                    ClientSecrets={new Secret("secret".Sha256())},//in production, you will have a secret key
                    AllowedGrantTypes=GrantTypes.Code, //more than one grant type can be used
                    RedirectUris={"https://localhost:44392/signin-oidc"},
                    PostLogoutRedirectUris={ "https://localhost:44392/signout-callback-oidc" },
                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        "mangoAdmin"
                    }
                }

            };

    }
}
