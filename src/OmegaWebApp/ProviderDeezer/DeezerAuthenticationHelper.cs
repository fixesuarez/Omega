using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace OmegaWebApp.ProviderDeezer
{
    public static class DeezerAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier( [NotNull] JObject user ) => user.Value<string>( "id" );

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetName( [NotNull] JObject user ) => user.Value<string>( "name" );

        /// <summary>
        /// Gets the URL corresponding to the authenticated user.
        /// </summary>
        public static string GetLink( [NotNull] JObject user ) => user.Value<string>( "link" );

        /// <summary>
        /// Gets the profile picture URL corresponding to the authenticated user.
        /// </summary>
        public static string GetProfilePictureUrl( [NotNull] JObject user ) => user.Value<string>( "picture" );
    }
}
