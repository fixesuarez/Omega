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

        public async Task InsertAmbiance(string user, string name,string cover, string metaDonnees)
        {
            JObject rss = JObject.Parse(metaDonnees);

            Ambiance mood = new Ambiance(user, name);
            mood.Cover = cover;
            mood.Accousticness = (string)rss["Accousticness"];
            mood.Danceability = (string)rss["Danceability"];
            mood.Energy = (string)rss["Energy"];
            mood.Instrumentalness = (string)rss["Instrumentalness"];
            mood.Liveness = (string)rss["Liveness"];
            mood.Loudness = (string)rss["Loudness"];
            mood.Speechiness = (string)rss["Speechiness"];
            mood.Popularity = (string)rss["Popularity"];
            TableOperation insertOperation = TableOperation.Insert(mood);

            await _table.ExecuteAsync(insertOperation);
        }

        public async Task InsertAmbiance(string user, string name, string cover, MetaDonnees metaDonnees)
        {
            if(await RetrieveAmbiance(user, name) == null)
            {
                Ambiance mood = new Ambiance(user, name);
                mood.Cover = cover;
                mood.Accousticness = metaDonnees.accousticness;
                mood.Danceability = metaDonnees.danceability;
                mood.Energy = metaDonnees.energy;
                mood.Instrumentalness = metaDonnees.instrumentalness;
                mood.Liveness = metaDonnees.liveness;
                mood.Loudness = metaDonnees.loudness;
                mood.Mode = metaDonnees.mode;
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
            MetaDonnees lounge = new MetaDonnees(null, null, null, "0.10", null, null, "0.2", null, "50", null);
            MetaDonnees energy = new MetaDonnees(null, "0.9", null, null, null, null, null, null, null, null);
            MetaDonnees dance = new MetaDonnees("0.9", "0.9", null, null, null, null, null, "1", null, null);
            MetaDonnees mad = new MetaDonnees(null, "0.9", null, null, null, null, null, null, null, null);
            string coverLounge = "http://park-place-hotel.com/assets/uploads/photo-gallery/beacon-lounge-horz.jpg";
            string coverEnergy = "http://www.lockheedmartin.com/content/dam/lockheed/data/corporate/photo/features/2015/energy-lightning.jpg";
            string coverDance = "https://wildlyfreewoman.files.wordpress.com/2011/04/creative-dance.jpg";
            string coverMad = "http://www.citylit.ac.uk/media/catalog/product/cache/1/image/1280x/040ec09b1e35df139433887a97daa66f/m/u/munch_2048_1.jpg";
            await InsertAmbiance("allUser", "Lounge",coverLounge, lounge);
            await InsertAmbiance("allUser", "Energy", coverEnergy, energy);
            await InsertAmbiance("allUser", "Dance", coverDance, dance);
            await InsertAmbiance("allUser", "Mad", coverMad, mad);
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

            query = new TableQuery<Ambiance>()
        .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "allUser"));

            query.TakeCount = 1000;
            tableContinuationToken = null;
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
