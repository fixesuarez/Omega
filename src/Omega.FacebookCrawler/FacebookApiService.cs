using Facebook;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.FacebookCrawler
{
    public class FacebookApiService
    {
        public async Task GetAllFacebookGroups( string accessToken )
        {
            FacebookClient client = new FacebookClient( accessToken );
            dynamic response = await client.GetTaskAsync( "/v2.8/me/groups?fields=id,name,cover,members{id,name,email}" );
            JObject groupsJson = JObject.FromObject( response );
            JArray groups = (JArray) response["data"];

            foreach( var group in groups )
            {
                string groupId = (string) group["id"];
                string groupName = (string) group["name"];
                string groupCover = (string) group["cover"]["source"];
                List<User> groupMembers = new List<User>();
                JArray members = (JArray) group["members"]["data"];
                foreach( var member in members )
                {
                    string email = (string) member["email"];
                    string id = (string) member["id"];
                    if( email != null )
                    {
                        User u = new User();
                        u.PartitionKey = string.Empty;
                        u.RowKey = email;
                        u.FacebookId = id;
                        groupMembers.Add( u );
                    }
                }
            }
        }
        
        public async Task GetAllFacebookEvents( string accessToken )
        {
            FacebookClient fbClient = new FacebookClient( accessToken );

            dynamic result = await fbClient.GetTaskAsync( "/me/events?fields=cover,id,name,attending{id,email,name}" );
            JObject eventsJson = JObject.FromObject( result );
            JArray events = (JArray) result["data"];

            foreach( var _event in events )
            {
                string eventId = (string) _event["id"];
                string eventName = (string) _event["name"];
                string groupCover = (string) _event["cover"]["source"];
                List<User> eventAttendings = new List<User>();
                JToken attendingTokens = _event["attending"]["data"];
                foreach( var attending in attendingTokens )
                {
                    string email = (string) attending["email"];
                    string id = (string) attending["id"];
                    if( email != null )
                    {
                        User u = new User();
                        u.PartitionKey = string.Empty;
                        u.RowKey = email;
                        u.FacebookId = id;
                        eventAttendings.Add( u );
                    }
                }
            }
        }
    }
}
