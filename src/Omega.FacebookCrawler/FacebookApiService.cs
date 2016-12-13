﻿using Facebook;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using System;
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
                string groupCover = null;
                if( groupCoverJToken != null )
                    groupCover = (string) group["cover"]["source"];
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
            FacebookClient fbClient = new FacebookClient( "accessToken" );

            dynamic result = await fbClient.GetTaskAsync( "/v2.8/me/events?fields=id,name,start_time,cover,attending{id,email,name}" );
            JObject eventsJson = JObject.FromObject( result );
            JArray events = (JArray) eventsJson["data"];

            foreach( var _event in events )
            {
                string startTime = (string) _event["start_time"];
                string day = startTime.Substring( 8, 2 );
                int dayInt = int.Parse( day );
                string month = startTime.Substring( 5, 2 );
                int monthInt = int.Parse( month );
                string year = startTime.Substring( 0, 4 );
                int yearInt = int.Parse( year );
                DateTime today = DateTime.Today;
                DateTime dateEvent = new DateTime( yearInt, monthInt, dayInt );
                today.AddDays( 3 );
                if( DateTime.Compare( today, dateEvent ) < 0 )
                {
                    string eventId = (string) _event["id"];
                    string eventName = (string) _event["name"];
                    JToken eventCoverJToken = _event["cover"];
                    string eventCover = null;
                    if( eventCoverJToken != null )
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
}