using Facebook;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.FacebookCrawler
{
    public class FacebookApiService
    {
        EventGroupGateway _eventGroupGateway;
        UserGateway _userGateway;

        public FacebookApiService(EventGroupGateway eventGroupGateway, UserGateway userGateway )
        {
            _eventGroupGateway = eventGroupGateway;
            _userGateway = userGateway;
        }

        public async Task GetAllFacebookGroups( string email )
        {
            string accessToken = await _userGateway.FindFacebookAccessToken( email );
            FacebookClient client = new FacebookClient( accessToken );
            dynamic response = await client.GetTaskAsync( "/v2.8/me/groups?fields=id,name,cover,members{id,name,email}" );
            JObject groupsJson = JObject.FromObject( response );
            JArray groups = (JArray) groupsJson["data"];

            foreach( var group in groups )
            {
                string groupId = (string) group["id"];
                string groupName = (string) group["name"];
                JToken groupCoverJToken = group["cover"];
                string eventCover = null;
                if( groupCoverJToken != null )
                    eventCover = (string) group["cover"]["source"];
                List<User> groupMembers = new List<User>();
                JArray members = (JArray) group["members"]["data"];
                foreach( var member in members )
                {
                    string mail = (string) member["email"];
                    string id = (string) member["id"];
                    if( mail != null )
                    {
                        User u = new User();
                        u.PartitionKey = string.Empty;
                        u.RowKey = mail;
                        u.FacebookId = id;
                        groupMembers.Add( u );
                    }
                }
                await _eventGroupGateway.InsertEventGroup( groupId, groupMembers, "group", groupCover );
            }
        }
        
        public async Task GetAllFacebookEvents( string email )
        {
            string accessToken = await _userGateway.FindFacebookAccessToken( email );
            FacebookClient fbClient = new FacebookClient( "EAACEdEose0cBAC0o91sSKXme7hsDDvU4kvN2qVd3UBPZAAbff3Ub7qe2aVj4DI7v8P9NaVFGWnqB3UNPSa59xqfo03niHrG0WvG7wMtZCoOKwQw3ATEuO6fNBxnKJuJLK1GrpJasxZA7D7K5Ahqt2w1rk8Odg4EN4Biu69c3AZDZD" );

            dynamic result = await fbClient.GetTaskAsync( "/v2.8/me/events?fields=id,name,cover,attending{id,email,name}" );
            JObject eventsJson = JObject.FromObject( result );
            JArray events = (JArray) eventsJson["data"];

            foreach( var _event in events )
            {
                string eventId = (string) _event["id"];
                string eventName = (string) _event["name"];
                JToken eventCoverJToken = _event["cover"];
                string eventCover = null;
                if( eventCoverJToken != null)
                    eventCover = (string) _event["cover"]["source"];
                List<User> eventAttendings = new List<User>();
                JToken attendingTokens = _event["attending"]["data"];
                foreach( var attending in attendingTokens )
                {
                    string mail = (string) attending["email"];
                    string id = (string) attending["id"];
                    if( mail != null )
                    {
                        User u = new User();
                        u.PartitionKey = string.Empty;
                        u.RowKey = mail;
                        u.FacebookId = id;
                        eventAttendings.Add( u );
                    }
                }
                await _eventGroupGateway.InsertEventGroup( eventId, eventAttendings, "event", eventCover );
            }
        }
    }
}
