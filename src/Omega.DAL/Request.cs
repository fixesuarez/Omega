using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class Requests
    {
        public CloudTable ConnectCleanTrackTable(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("CleanTrack");

            return table;
        }

        public CloudTable ConnectAmbianceTable(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("Ambiance");

            return table;
        }

        public async void AddSongCleanTrack(MetaDonnees meta, string artist, string deezerId, string trackId, string title, string source, string AlbumName, string popularity, string connectionString)
        {
            source = source.Substring(0, 1);
            CloudTable table = ConnectCleanTrackTable(connectionString);
            string name = source + ":" + trackId;
            CleanTrack cleanT = await GetSongCleanTrack(name, connectionString);

            if (cleanT.Id == null)
            {
                CleanTrack track = new CleanTrack(trackId, source);
                track.Artist = artist;
                track.DeezerId = deezerId;
                track.Id = trackId;
                track.Source = source;
                track.Title = title;
                track.Danceability = meta.danceability;
                track.Loudness = meta.loudness;
                track.Mode = meta.mode;
                track.Speechiness = meta.speechiness;
                track.Acousticness = meta.acousticness;
                track.Instrumentalness = meta.instrumentalness;
                track.Liveness = meta.liveness;
                track.Valence = meta.valence;
                track.Tempo = meta.tempo;
                track.AlbumName = AlbumName;
                track.Popularity = popularity;
                track.Energy = meta.energy;

                TableOperation insertOperation = TableOperation.Insert(track);

                await table.ExecuteAsync(insertOperation);
            }
        }

        public async Task<CleanTrack> GetSongCleanTrack(string trackIdSource, string connectionString)
        {
            CleanTrack ct = new CleanTrack();

            CloudTable table = ConnectCleanTrackTable(connectionString);

            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>("", trackIdSource);

            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
                ct = (CleanTrack)retrievedResult.Result;

            return ct;
        }

        public async void UpdateCleanTrack(MetaDonnees meta, string trackId, string title, string source, string AlbumName, string popularity, string connectionString, string artist)
        {
            CloudTable table = ConnectCleanTrackTable(connectionString);

            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>("", source + ":" + trackId);

            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            CleanTrack updateEntity = (CleanTrack)retrievedResult.Result;

            if (updateEntity != null)
            {
                CleanTrack track = new CleanTrack(trackId, source);
                updateEntity.Id = trackId;
                updateEntity.Source = source;
                updateEntity.Title = title;
                updateEntity.Danceability = meta.danceability;
                updateEntity.Loudness = meta.loudness;
                updateEntity.Mode = meta.mode;
                updateEntity.Speechiness = meta.speechiness;
                updateEntity.Acousticness = meta.acousticness;
                updateEntity.Instrumentalness = meta.instrumentalness;
                updateEntity.Liveness = meta.liveness;
                updateEntity.Valence = meta.valence;
                updateEntity.Tempo = meta.tempo;
                updateEntity.AlbumName = AlbumName;
                updateEntity.Popularity = popularity;
                updateEntity.Energy = meta.energy;
                updateEntity.Artist = artist;

                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                await table.ExecuteAsync(updateOperation);
            }
        }

        public async void DeleteTrack(string trackIdSource, string connectionString)
        {
            CleanTrack ct = new CleanTrack();

            CloudTable table = ConnectCleanTrackTable(connectionString);

            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>("", trackIdSource);

            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            CleanTrack deleteEntity = (CleanTrack)retrievedResult.Result;

            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                await table.ExecuteAsync(deleteOperation);
            }
        }

        // Model Requests

        public async void AddMood(string user, string name, string metaDonnees, string connectionString)
        {
            CloudTable table = ConnectAmbianceTable(connectionString);
            JObject rss = JObject.Parse(metaDonnees);

            Ambiance mood = new Ambiance(user, name);
            mood.Acousticness = (string)rss["Acousticness"];
            mood.Danceability = (string)rss["Danceability"];
            mood.Energy = (string)rss["Energy"];
            mood.Instrumentalness = (string)rss["Instrumentalness"];
            mood.Liveness = (string)rss["Liveness"];
            mood.Loudness = (string)rss["Loudness"];
            mood.Mode = (string)rss["Mode"];
            mood.Popularity = (string)rss["Popularity"];
            TableOperation insertOperation = TableOperation.Insert(mood);

            await table.ExecuteAsync(insertOperation);
        }
    }
}