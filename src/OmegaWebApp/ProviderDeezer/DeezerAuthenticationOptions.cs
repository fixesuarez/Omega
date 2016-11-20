using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OmegaWebApp.ProviderDeezer
{
    public class DeezerAuthenticationOptions : OAuthOptions
    {
        public DeezerAuthenticationOptions()
        {
            AuthenticationScheme = DeezerAuthenticationDefaults.AuthenticationScheme;
            DisplayName = DeezerAuthenticationDefaults.DisplayName;
            ClaimsIssuer = DeezerAuthenticationDefaults.Issuer;

            CallbackPath = new PathString( DeezerAuthenticationDefaults.CallbackPath );

            AuthorizationEndpoint = DeezerAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DeezerAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DeezerAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
