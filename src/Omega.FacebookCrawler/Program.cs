using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Omega.DAL;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.FacebookCrawler
{
    public class Program
    {
        static EventGroupGateway _eventGroupGateway;
        static FacebookApiService _facebookApiService;
        static UserGateway _userGateway;
        static IConfigurationRoot _configuration;
        public static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            _eventGroupGateway = new EventGroupGateway(_configuration["data:azure:ConnectionString"]);
            _userGateway = new UserGateway(_configuration["data:azure:ConnectionString"]);
            _facebookApiService = new FacebookApiService(_eventGroupGateway, _userGateway);

            Crawl().Wait();
        }

        public static async Task Crawl()
        {
            for (;;)
            {
                try
                {
                    await UpdateTable();
                }
                catch (Exception)
                {
                    //need log
                }
            }
        }

        public static async Task CheckQueue()
        {
            CloudQueueMessage message;
            while ((message = await _eventGroupGateway.GetMessagePriorityQueue()) != null)
            {
                await _facebookApiService.GetAllFacebookEvents(message.AsString);
                await Task.Delay(1000);
                await _facebookApiService.GetAllFacebookGroups(message.AsString);
                Console.WriteLine("PriorityQueue checked");
                await _eventGroupGateway.DeleteMessagePriorityQueue(message);
            }
            while ((message = await _eventGroupGateway.GetMessageNormalQueue()) != null)
            {
                await _facebookApiService.GetAllFacebookEvents(message.AsString);
                await Task.Delay(1000);
                await _facebookApiService.GetAllFacebookGroups(message.AsString);
                Console.WriteLine("NormalQueue checked");
                await _eventGroupGateway.DeleteMessageNormalQueue(message);
            }
        }

        public static async Task UpdateTable()
        {
            TableQuerySegment<EventGroup> tableQueryResult;
            tableQueryResult = await _eventGroupGateway.TableQueryResult();
            await CheckQueue();
            for (int i = 0; i < tableQueryResult.Results.Count; i++)
            {
                await CheckQueue();
                string email = tableQueryResult.Results[i].RowKey;
                await _facebookApiService.GetAllFacebookEvents(email);
                await Task.Delay(1000);
                await _facebookApiService.GetAllFacebookGroups(email);
                await Task.Delay(1000);
                Console.WriteLine("EventGroup checked");
            }
        }
    }
}
