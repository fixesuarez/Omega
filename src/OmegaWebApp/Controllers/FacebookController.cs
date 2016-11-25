using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using OmegaWebApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaWebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class FacebookController : Controller
    {
        private static readonly string FacebookGraphApi = "https://graph.facebook.com";
        readonly UserService _userService;

        // GET: /<controller>/
        public FacebookController( UserService userService )
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all the user's facebook events
        /// </summary>
        /// <returns>List of FacebookEvents</returns>
        [HttpGet( "Groups" )]
        public async Task<JToken> GetAllFacebookGroups()
        {
            using( HttpClient client = new HttpClient() )
            {
                List<GroupOrEventFacebook> groups = new List<GroupOrEventFacebook>();

                string email = User.FindFirst( ClaimTypes.Email ).Value;
                string accessToken = await _userService.GetFacebookAccessToken( email );
                string facebookId = await _userService.GetFacebookId( email );
                string parameters = Uri.EscapeDataString( "id,cover,link,name,members{email,id,name}" );

                Uri groupDetailUri = new Uri(
                string.Format( "{0}/me/groups?" +
                    "access_token={1}" +
                    "&debug=all" +
                    "&fields={2}" +
                    "&format=json&method=get&pretty=0&suppress_http_code=1",
                FacebookGraphApi,
                accessToken,
                parameters ) );

                HttpResponseMessage message = await client.GetAsync( groupDetailUri );

                using( Stream responseStream = await message.Content.ReadAsStreamAsync() )
                using( StreamReader reader = new StreamReader( responseStream ) )
                {
                    string groupsStringResponse = reader.ReadToEnd();
                    JObject groupsJsonResponse = JObject.Parse( groupsStringResponse );

                    foreach( var group in groupsJsonResponse["data"] )
                    {
                        string groupId = (string) group["id"];
                        string groupName = (string) group["name"];
                        string groupCover = (string) group["cover"]["source"];

                        GroupOrEventFacebook fbGroup = new GroupOrEventFacebook( groupId, groupName, groupCover );
                        groups.Add( fbGroup );
                    }
                    string groupsString = JsonConvert.SerializeObject( groups );
                    JToken groupsJson = JToken.Parse( groupsString );

                    return groupsJson;

                }
            }
        }
    }
}