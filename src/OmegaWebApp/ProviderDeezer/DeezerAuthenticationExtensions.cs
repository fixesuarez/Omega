using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace OmegaWebApp.ProviderDeezer
{
    public static class DeezerAuthenticationExtensions
    {
        /// <summary>
        /// Adds the <see cref="DeezerAuthenticationMiddleware"/> middleware to the specified
        /// <see cref="IApplicationBuilder"/>, which enables Deezer authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="DeezerAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseDeezerAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] DeezerAuthenticationOptions options )
        {
            if (app == null)
            {
                throw new ArgumentNullException( nameof( app ) );
            }

            if (options == null)
            {
                throw new ArgumentNullException( nameof( options ) );
            }

            return app.UseMiddleware<DeezerAuthenticationMiddleware>( Options.Create( options ) );
        }

        /// <summary>
        /// Adds the <see cref="DeezerAuthenticationMiddleware"/> middleware to the specified
        /// <see cref="IApplicationBuilder"/>, which enables Deezer authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="DeezerAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseDeezerAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<DeezerAuthenticationOptions> configuration )
        {
            if (app == null)
            {
                throw new ArgumentNullException( nameof( app ) );
            }

            if (configuration == null)
            {
                throw new ArgumentNullException( nameof( configuration ) );
            }

            var options = new DeezerAuthenticationOptions();
            configuration( options );

            return app.UseMiddleware<DeezerAuthenticationMiddleware>( Options.Create( options ) );
        }
    }
}
