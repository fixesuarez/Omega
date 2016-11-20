using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace OmegaWebApp.Authentication
{
    public static class OAuthCreatingTicketContextExtensions
    {
        public static string GetEmail( this OAuthCreatingTicketContext @this )
        {
            return @this.Identity.FindFirst( c => c.Type == ClaimTypes.Email ).Value;
        }

        public static string GetSpotifyEmail( this OAuthCreatingTicketContext @this )
        {
            JToken userJson = @this.User;
            return (string) userJson["email"];
        }

        public static string GetId( this OAuthCreatingTicketContext @this )
        {
            return @this.Identity.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value;
        }

        //public static string GetAccessToken( this OAuthCreatingTicketContext @this)
        //{
        //    return @this.Identity.FindFirst(c => c.Type == ClaimTypes.).Value;
        //}
    }
}
