using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class MixTableGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableMixTable;

        public MixTableGateway(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableMixTable = _tableClient.GetTableReference("MixTable");
            _tableMixTable.CreateIfNotExistsAsync().Wait();
        }

        public async Task<Mix> RetrieveMix(string mixName, string guid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Mix>(guid, mixName);
            TableResult retrievedMix = await _tableMixTable.ExecuteAsync(retrieveOperation);
            return (Mix) retrievedMix.Result;
        }

        public async Task InsertMix(Mix mix, string guid)
        {
            if(await RetrieveMix(mix.PartitionKey, guid) == null)
            {
                TableOperation insert = TableOperation.Insert(mix);
                await _tableMixTable.ExecuteAsync(insert);
            }        
        }

        public async Task DeleteMix(string name, string guid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Mix>(guid, name);
            TableResult retrievedMix = await _tableMixTable.ExecuteAsync(retrieveOperation);
            Mix deleteEntity = (Mix)retrievedMix.Result;

            if (deleteEntity != null)
            {
                TableOperation delete = TableOperation.Delete(deleteEntity);
                await _tableMixTable.ExecuteAsync(delete);
            }
        }

        public async Task<List<Mix>> RetrieveAllMixUser(string userName)
        {
            List<Mix> mixs = new List<Mix>();
            TableQuery<Mix> query = new TableQuery<Mix>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userName));

            query.TakeCount = 1000;
            TableContinuationToken tableContinuationToken = null;
            do
            {
                var queryResponse = await _tableMixTable.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                tableContinuationToken = queryResponse.ContinuationToken;
                mixs.AddRange(queryResponse.Results);
            } while (tableContinuationToken != null);

            return mixs;
        }
    }
}
