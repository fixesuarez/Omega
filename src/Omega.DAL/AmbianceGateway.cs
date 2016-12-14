using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class AmbianceGateway
    {
        CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;
        CloudTable _table;
        public AmbianceGateway(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            _tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            _table = _tableClient.GetTableReference("Ambiance");

            // Create the table if it doesn't exist.
            _table.CreateIfNotExistsAsync();

            Ambiance retrievedAmbiance = RetrieveAmbiance("allUser", "Lounge").Result;
            if (retrievedAmbiance == null)
            {
                InsertAllUserAmbiance().Wait();
            }
        }

        public async Task InsertAmbiance(string user, string name, string metaDonnees)
        {
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

            await _table.ExecuteAsync(insertOperation);
        }

        public async Task InsertAmbiance(string user, string name, MetaDonnees metaDonnees)
        {
            if(await RetrieveAmbiance(user, name) == null)
            {
                Ambiance mood = new Ambiance(user, name);
                mood.Acousticness = metaDonnees.acousticness;
                mood.Danceability = metaDonnees.danceability;
                mood.Energy = metaDonnees.energy;
                mood.Instrumentalness = metaDonnees.instrumentalness;
                mood.Liveness = metaDonnees.liveness;
                mood.Loudness = metaDonnees.loudness;
                mood.Mode = metaDonnees.mode;
                mood.Tempo = metaDonnees.tempo;
                mood.Speechiness = metaDonnees.speechiness;
                mood.Popularity = metaDonnees.popularity;
                TableOperation insertOperation = TableOperation.Insert(mood);
                await _table.ExecuteAsync(insertOperation);
            }
        }

        public async Task DeleteAmbiance(string user, string ambianceName)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Ambiance>(user, ambianceName);
            TableResult retrievedResult = await _table.ExecuteAsync(retrieveOperation);
            Ambiance deleteEntity = (Ambiance)retrievedResult.Result;
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                await _table.ExecuteAsync(deleteOperation);
            }
        }

        public async Task<Ambiance> RetrieveAmbiance(string user, string ambianceName)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Ambiance>(user, ambianceName);
            TableResult retrievedResult = await _table.ExecuteAsync(retrieveOperation);
            Ambiance retrieveEntity = (Ambiance)retrievedResult.Result;
            return retrieveEntity;
        }

        public async Task InsertAllUserAmbiance()
        {
            MetaDonnees lounge = new MetaDonnees("", "", "", "0.10", "", "", "0.2", "", "50", "");
            MetaDonnees energy = new MetaDonnees("", "0.9", "", "", "", "", "", "", "", "");
            MetaDonnees dance = new MetaDonnees("0.9", "0.9", "", "", "", "", "", "1", "", "");
            MetaDonnees mad = new MetaDonnees("0.9", "0.9", "", "", "", "", "", "", "", "");

            await InsertAmbiance("allUser", "Lounge", lounge);
            await InsertAmbiance("allUser", "Energy", energy);
            await InsertAmbiance("allUser", "Dance", dance);
            await InsertAmbiance("allUser", "Mad", mad);
        }

        public async Task<List<Ambiance>> RetrieveAllUserAmbiance(string userName)
        {
            List<Ambiance> ambiances = new List<Ambiance>();
            TableQuery<Ambiance> query = new TableQuery<Ambiance>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userName));

            query.TakeCount = 1000;
            TableContinuationToken tableContinuationToken = null;
            do
            {
                var queryResponse = await _table.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                tableContinuationToken = queryResponse.ContinuationToken;
                ambiances.AddRange(queryResponse.Results);
            } while (tableContinuationToken != null);
            return ambiances;
        }
    }
}
