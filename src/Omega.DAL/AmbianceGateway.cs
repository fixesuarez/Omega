using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
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

            //if(RetrieveAmbiance("allUser","Lounge") == null)
            //{
            //    InsertAllUserAmbiance().Wait();
            //}
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
            Ambiance mood = new Ambiance(user, name);
            mood.Acousticness = metaDonnees.acousticness;
            mood.Danceability = metaDonnees.danceability;
            mood.Energy = metaDonnees.energy;
            mood.Instrumentalness = metaDonnees.instrumentalness;
            mood.Liveness = metaDonnees.liveness;
            mood.Loudness = metaDonnees.loudness;
            mood.Mode = metaDonnees.mode;
            mood.Popularity = metaDonnees.popularity;
            TableOperation insertOperation = TableOperation.Insert(mood);

            await _table.ExecuteAsync(insertOperation);
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
            await InsertAmbiance("allUser", "Energy", lounge);
            await InsertAmbiance("allUser", "Dance", lounge);
            await InsertAmbiance("allUser", "Mad", lounge);
        }
    }
}
