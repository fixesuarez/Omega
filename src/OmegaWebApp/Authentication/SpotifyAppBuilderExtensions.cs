using AspNet.Security.OAuth.Spotify;
using Microsoft.AspNetCore.Builder;
using System;

namespace OmegaWebApp.Authentication
{
    public static class SpotifyAppBuilderExtensions
    {
        public static IApplicationBuilder UseSpotifyAuthentication( this IApplicationBuilder app, Action<SpotifyAuthenticationOptions> configuration )
        {
            SpotifyAuthenticationOptions options = new SpotifyAuthenticationOptions();
            configuration( options );
            app.UseSpotifyAuthentication( options );
            return app;
        }
    }
}
