﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Omega.DAL;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;

namespace PlaylistCrawler
{
    public class Program
    {
        static IConfigurationRoot _configuration;
        static TrackGateway _trackGateway;
        static PlaylistGateway _playlistGateway;
        static SpotifyApiService _spotifyApiService;
        static DeezerApiService _deezerApiService;
        static UserGateway _userGateway;
        static CleanTrackGateway _cleanTrackGateway;
        public static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true)
               .AddEnvironmentVariables()
               .Build();
            _playlistGateway = new PlaylistGateway(_configuration["data:azure:ConnectionString"]);
            _trackGateway = new TrackGateway(_configuration["data:azure:ConnectionString"]);
            _userGateway = new UserGateway(_configuration["data:azure:ConnectionString"]);
            _cleanTrackGateway = new CleanTrackGateway(_configuration["data:azure:ConnectionString"]);
            _spotifyApiService = new SpotifyApiService(_trackGateway, _playlistGateway, _userGateway, _cleanTrackGateway);
            _deezerApiService = new DeezerApiService(_trackGateway, _playlistGateway, _userGateway, _cleanTrackGateway);
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
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public static async Task UpdateTable()
        {
            TableQuerySegment<User> tableQueryResult;
            tableQueryResult = await _userGateway.TableQueryResult();
            await CheckQueue();
            string guid = null;
            for (int i = 0; i < tableQueryResult.Results.Count; i++)
            {
                try
                {
                    await CheckQueue();
                    guid = tableQueryResult.Results[i].RowKey;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                
                if (!string.IsNullOrEmpty(tableQueryResult.Results[i].DeezerAccessToken))
                {
                    try
                    {
                        await _deezerApiService.GetAllDeezerPlaylists(guid);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                if (!string.IsNullOrEmpty(tableQueryResult.Results[i].SpotifyRefreshToken))
                {
                    try
                    {
                        await _spotifyApiService.GetSpotifyPlaylist(guid);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                await Task.Delay(1500);
                Console.WriteLine("Table checked");
            }
        }

        public static async Task CheckQueue()
        {
            CloudQueueMessage message;
            while ((message = await _userGateway.GetQueuMessage()) != null)
            {
                try
                {
                    User user = await _userGateway.FindUser(message.AsString);
                    if (!string.IsNullOrEmpty(user.DeezerAccessToken))
                    {
                        await _deezerApiService.GetAllDeezerPlaylists(user.Guid);
                    }
                    if (!string.IsNullOrEmpty(user.SpotifyRefreshToken))
                    {
                        await _spotifyApiService.GetSpotifyPlaylist(user.Guid);
                    }
                    Console.WriteLine("Queue checked");
                }
                catch (Exception)
                {
                    Console.WriteLine("User do not exist");
                }
                await Task.Delay(1500);
                await _userGateway.DeleteMessageQueue(message);
            }
        }
    }
}
