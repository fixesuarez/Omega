﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class PlaylistGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tablePlaylist;
        readonly CloudTable _tableTrack;

        public PlaylistGateway( string connectionString )
        {
            _storageAccount = CloudStorageAccount.Parse( connectionString );
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tablePlaylist = _tableClient.GetTableReference( "Playlist" );
            _tablePlaylist.CreateIfNotExistsAsync().Wait();
            _tableTrack = _tableClient.GetTableReference( "Track" );
            _tableTrack.CreateIfNotExistsAsync().Wait();
        }

        public async Task InsertPlaylist( Playlist p )
        {
            try
            {
                TableOperation retrievePlaylistOperation = TableOperation.Retrieve<Playlist>(p.PartitionKey, p.RowKey);

                TableResult retrievedResult = await _tablePlaylist.ExecuteAsync(retrievePlaylistOperation);
                if (retrievedResult.Result == null)
                {
                    TableBatchOperation batchOperation = new TableBatchOperation();
                    batchOperation.Insert(p);
                    await _tablePlaylist.ExecuteBatchAsync(batchOperation);
                }
                else if (retrievedResult.Result != null)
                {
                    Playlist updateEntity = (Playlist)retrievedResult.Result;
                    updateEntity.Cover = p.Cover;
                    updateEntity.Name = p.Name;
                    updateEntity.Pseudo = p.Pseudo;
                    TableOperation updateOperation = TableOperation.Replace(updateEntity);
                    await _tablePlaylist.ExecuteAsync(updateOperation);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task<List<Playlist>> RetrieveAllPlaylistsFromUser( string spotifyId, string deezerId )
        {
            try
            {
                List<Playlist> playlists = new List<Playlist>();
                if (spotifyId != null)
                {
                    var cond = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, spotifyId);
                    TableQuery<Playlist> query = new TableQuery<Playlist>();
                    query = query.Where(cond);

                    query.TakeCount = 1000;
                    TableContinuationToken tableContinuationToken = null;
                    do
                    {
                        var queryResponse = await _tablePlaylist.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                        tableContinuationToken = queryResponse.ContinuationToken;
                        playlists.AddRange(queryResponse.Results);
                    } while (tableContinuationToken != null);
                }
                if (deezerId != null)
                {
                    TableQuery<Playlist> query = new TableQuery<Playlist>()
                        .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, deezerId));

                    query.TakeCount = 1000;
                    TableContinuationToken tableContinuationToken = null;
                    do
                    {
                        var queryResponse = await _tablePlaylist.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                        tableContinuationToken = queryResponse.ContinuationToken;
                        playlists.AddRange(queryResponse.Results);
                    } while (tableContinuationToken != null);
                }
                foreach (Playlist p in playlists)
                {
                    p.Tracks = await RetrieveTracksFromPlaylists(p);
                }
                return playlists;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<Track>> RetrieveTracksFromPlaylists( Playlist p )
        {
            try
            {
                List<Track> tracks = new List<Track>();
                try
                {
                    var cond = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, p.PlaylistId);
                    TableQuery<Track> query = new TableQuery<Track>()
                            .Where(cond);

                    query.TakeCount = 1000;
                    TableContinuationToken tableContinuationToken = null;
                    do
                    {
                        var queryResponse = await _tableTrack.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                        tableContinuationToken = queryResponse.ContinuationToken;
                        tracks.AddRange(queryResponse.Results);
                    } while (tableContinuationToken != null);
                    return tracks;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
