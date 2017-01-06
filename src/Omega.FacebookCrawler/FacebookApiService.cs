using Facebook;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Omega.DAL.Event;

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
        public FacebookApiService() { }

        public async Task GetAllFacebookGroups( string guid )
        {
            User user = _userGateway.FindUser(guid).Result;
            DateTime userLastConnection = user.Timestamp.DateTime;
            if (DateTime.Compare(userLastConnection.AddDays(30), DateTime.Now) > 0)
            {
                string accessToken = await _userGateway.FindFacebookAccessToken(guid);
                FacebookClient client = new FacebookClient(accessToken);
                dynamic response = await client.GetTaskAsync("/v2.8/me/groups?fields=id,name,cover,members{id,name,email}");
                JObject groupsJson = JObject.FromObject(response);
                JArray groups = (JArray)groupsJson["data"];
                await _userGateway.UpdateUserGroups(guid, groups);

                foreach (var group in groups)
                {
                    string groupId = (string)group["id"];
                    string groupName = (string)group["name"];
                    JToken groupCoverJToken = group["cover"];
                    string groupCover = null;
                    if (groupCoverJToken != null)
                        groupCover = (string)group["cover"]["source"];
                    List<User> groupMembers = new List<User>();
                    JArray members = (JArray)group["members"]["data"];
                    foreach (var member in members)
                    {
                        string mail = (string)member["email"];
                        string id = (string)member["id"];
                        if (mail != null)
                        {
                            User u = new User();
                            u.PartitionKey = string.Empty;
                            u.RowKey = guid;
                            u.FacebookId = id;
                            groupMembers.Add(u);
                        }
                    }
                    await _eventGroupGateway.InsertEventGroup(groupId, groupMembers, "group", groupCover, groupName);
                }
            }
        }
        
        public async Task GetAllFacebookEvents( string guid )
        {
            User user = _userGateway.FindUser(guid).Result;
            DateTime userLastConnection = user.Timestamp.DateTime;

            List<Event> cleanEvents = new List<Event>();
            Event cleanEvent = new Event();
            List<Attendings> attendings = new List<Attendings>();
            Attendings attendingA = new Attendings();

            if (DateTime.Compare(userLastConnection.AddDays(30), DateTime.Now) > 0)
            {
                string accessToken = await _userGateway.FindFacebookAccessToken(guid);
                FacebookClient fbClient = new FacebookClient(accessToken);

                dynamic result = await fbClient.GetTaskAsync("/v2.8/me/events?fields=id,name,start_time,cover,attending{id,email,name}");
                JObject eventsJson = JObject.FromObject(result);
                JArray events = (JArray)eventsJson["data"];
                //await _userGateway.UpdateUserEvents(guid, events);


                foreach (var _event in events)
                {
                    string startTime = (string)_event["start_time"];
                    string day = startTime.Substring(8, 2);
                    int dayInt = int.Parse(day);
                    string month = startTime.Substring(5, 2);
                    int monthInt = int.Parse(month);
                    string year = startTime.Substring(0, 4);
                    int yearInt = int.Parse(year);
                    DateTime today = DateTime.Today;
                    DateTime dateEvent = new DateTime(yearInt, monthInt, dayInt);
                    today.AddDays(3);


                    if (DateTime.Compare(today, dateEvent) < 0)
                    {
                        string eventId = (string)_event["id"];
                        string eventName = (string)_event["name"];
                        JToken eventCoverJToken = _event["cover"];
                        string eventCover = null;
                        if (eventCoverJToken != null)
                            eventCover = (string)_event["cover"]["source"];

                        List<User> eventAttendings = new List<User>();
                        JToken attendingTokens = _event["attending"]["data"];
                        
                        foreach (var attending in attendingTokens)
                        {
                            string mail = (string)attending["email"];
                            string id = (string)attending["id"];
                            if (mail != null)
                            {
                                cleanEvent = new Event();
                                User u = new User();
                                u.PartitionKey = string.Empty;
                                u.RowKey = guid;
                                u.FacebookId = id;
                                eventAttendings.Add(u);
                                attendingA.Email = mail;
                                attendingA.FacebookId = id;
                                attendings.Add(attendingA);
                                cleanEvent.attendings = attendings;
                                cleanEvent.Cover = eventCover;
                                cleanEvent.Id = eventId;
                                cleanEvent.Name = eventName;
                                cleanEvent.StartTime = dateEvent;
                                cleanEvents.Add(cleanEvent);
                            }
                        }
                        await _eventGroupGateway.InsertEventGroup(eventId, eventAttendings, "event", eventCover, eventName, dateEvent);     
                    }                 
                }
            }
            await _userGateway.UpdateUserEvents(guid, cleanEvents);
        }
    }
}
