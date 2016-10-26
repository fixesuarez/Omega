using Microsoft.AspNetCore.Builder;
using System;

namespace OmegaWebApp.Authentication
{
    public static IApplicationBuilder UseSpotifyAuthentication( this IApplicationBuilder app, Action<SpotifyOptions> configuration )
    {
        SpotifyOptions options = new SpotifyOptions();
        configuration( options );
        app.UseSpotifyAuthentication( options );
        return app;
    }
}
