﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Omega.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

// 100 commits
namespace Omega.Crawler
{
    class Program
    {
        static Controller ct = new Controller();

        static void Main(string[] args)
        {
            ct.GetDatabaseCreator().CreateCleanTrackTable(ct.Config()["data:azure:ConnectionString"]);
            Crawl().Wait();
        }

        static async Task Crawl()
        {
            for (;;)
            {
                try
                {
                    await BrowseTrack();
                }
                catch (Exception)
                {
                    //need log
                }
            }
        }

        public static async Task CheckQueu()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ct.Config()["data:azure:ConnectionString"]);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();

            CloudQueueMessage message;

            while ((message = await queue.GetMessageAsync()) != null)
            {
                string trackId;
                string source;
                if (message.AsString.Substring(0, 2) == "s:")
                {
                    trackId = message.AsString.Substring(2);
                    source = "s";
                    await ct.GetAnalyser().AnalyseNewSong(ct, trackId, source);
                }
                else if (message.AsString.Substring(0, 2) == "d:")
                {
                    trackId = message.AsString.Substring(2);
                    source = "d";
                    await ct.GetAnalyser().AnalyseNewSong(ct, trackId, source);
                }
                await queue.DeleteMessageAsync(message);
                Console.WriteLine("Queu Checked");
                Thread.Sleep(1000);
            }
        }

        public static async Task BrowseTrack()
        {
            //CloudTable table = ct.GetRequests().ConnectCleanTrackTable();
            string secretKey = ct.Config()["data:azure:ConnectionString"];
            CloudTable table = ct.GetRequests().ConnectCleanTrackTable(secretKey);
            TableContinuationToken continuationToken = null;
            TableQuery<CleanTrack> tableQuery = new TableQuery<CleanTrack>();
            TableQuerySegment<CleanTrack> tableQueryResult;

            do
            {
                tableQueryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                continuationToken = tableQueryResult.ContinuationToken;
                await CheckQueu();
                for (int i = 0; i < tableQueryResult.Results.Count; i++)
                {
                    await CheckQueu();
                    string trackId = tableQueryResult.Results[i].Id;
                    string source = tableQueryResult.Results[i].Source.Substring(0, 1);
                    await ct.GetAnalyser().AnalyseSong(ct, trackId, source);
                    Console.WriteLine("Table Checked");
                    await Task.Delay(1000);
                }
            } while (continuationToken != null);
        }
    }
}