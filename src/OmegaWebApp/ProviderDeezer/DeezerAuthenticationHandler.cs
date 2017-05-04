using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System.Text;
using System.Linq;

namespace OmegaWebApp.ProviderDeezer
{
    public class DeezerAuthenticationHandler : OAuthHandler<DeezerAuthenticationOptions>
    {
        public DeezerAuthenticationHandler( [NotNull] HttpClient client )
            : base( client )
        {
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync( string code, string redirectUri )
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "app_id", Options.ClientId },
                { "secret", Options.ClientSecret },
                { "code", code }
            };

            var requestContent = new FormUrlEncodedContent( tokenRequestParameters );

            var requestMessage = new HttpRequestMessage( HttpMethod.Post, Options.TokenEndpoint );
            requestMessage.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync( requestMessage, Context.RequestAborted );
            if( response.IsSuccessStatusCode )
            {
                string content = await response.Content.ReadAsStringAsync();
                var payload = new JObject(
                    content.Split( '&' )
                           .Select( s => GetProperty( s ) ) );
                return OAuthTokenResponse.Success( payload );
            }
            else
            {
                var error = "OAuth token endpoint failure: " + await Display( response );
                return OAuthTokenResponse.Failed( new Exception( error ) );
            }
        }

        JProperty GetProperty( string s )
        {
            string[] components = s.Split( '=' );
            return new JProperty( components[0], components[1] );
        }

        static async Task<string> Display( HttpResponseMessage response )
        {
            var output = new StringBuilder();
            output.Append( "Status: " + response.StatusCode + ";" );
            output.Append( "Headers: " + response.Headers.ToString() + ";" );
            output.Append( "Body: " + await response.Content.ReadAsStringAsync() + ";" );
            return output.ToString();
        }

        
        protected override async Task<AuthenticationTicket> CreateTicketAsync( [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens )
        {
            string uri = new StringBuilder()
                .Append( Options.UserInformationEndpoint )
                .AppendFormat( "?access_token={0}", tokens.AccessToken )
                .ToString();
            var request = new HttpRequestMessage( HttpMethod.Get, uri );
            request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );

            var response = await Backchannel.SendAsync( request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted );
            if( !response.IsSuccessStatusCode )
            {
                Logger.LogError( "An error occurred when retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync() );

                throw new HttpRequestException( "An error occurred when retrieving the user profile." );
            }

            var payload = JObject.Parse( await response.Content.ReadAsStringAsync() );

            identity.AddOptionalClaim( ClaimTypes.NameIdentifier, DeezerAuthenticationHelper.GetIdentifier( payload ), Options.ClaimsIssuer )
                    .AddOptionalClaim( ClaimTypes.Name, DeezerAuthenticationHelper.GetName( payload ), Options.ClaimsIssuer )
                    .AddOptionalClaim( "urn:deezer:url", DeezerAuthenticationHelper.GetLink( payload ), Options.ClaimsIssuer )
                    .AddOptionalClaim( "urn:deezer:profilepicture", DeezerAuthenticationHelper.GetProfilePictureUrl( payload ), Options.ClaimsIssuer );

            var principal = new ClaimsPrincipal( identity );
            var ticket = new AuthenticationTicket( principal, properties, Options.AuthenticationScheme );

            var context = new OAuthCreatingTicketContext( ticket, Context, Options, Backchannel, tokens, payload );
            await Options.Events.CreatingTicket( context );

            return context.Ticket;
        }

        protected override string BuildChallengeUrl( AuthenticationProperties properties, string redirectUri )
        {
            Dictionary<string, string> queryStrings = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );
            //queryStrings.Add( "response_type", "code" );
            queryStrings.Add( "app_id", Options.ClientId );
            queryStrings.Add( "redirect_uri", redirectUri );

            AddQueryString( queryStrings, properties, "perms", FormatScope() );

            string state = Options.StateDataFormat.Protect( properties );
            queryStrings.Add( "state", state );

            string authorizationEndpoint = QueryHelpers.AddQueryString( Options.AuthorizationEndpoint, queryStrings );
            return authorizationEndpoint;
        }

        protected override string FormatScope()
        {
            string scope = string.Join( ",", Options.Scope );
            return scope;
        }

        static void AddQueryString(
            IDictionary<string, string> queryStrings,
            AuthenticationProperties properties,
            string name,
            string defaultValue = null )
        {
            string value;
            if( !properties.Items.TryGetValue( name, out value ) )
            {
                value = defaultValue;
            }
            else
            {
                properties.Items.Remove( name );
            }

            if( value == null )
            {
                return;
            }

            queryStrings[name] = value;
        }
    }
}
