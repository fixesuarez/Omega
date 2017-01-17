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

        public async Task<Mix> RetrieveMix(string MixName, string guid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Mix>(MixName, guid);
            TableResult retrievedMix = await _tableMixTable.ExecuteAsync(retrieveOperation);
            return (Mix) retrievedMix.Result;
        }

        public async Task InsertMix(Mix mix, string guid)
        {
            if(await RetrieveMix(mix.PartitionKey, guid) != null)
            {
                TableOperation insert = TableOperation.Insert(mix);
                await _tableMixTable.ExecuteAsync(insert);
            }        
        }

        public async Task DeleteMix(string name, string guid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Mix>(name, guid);
            TableResult retrievedMix = await _tableMixTable.ExecuteAsync(retrieveOperation);
            if(retrievedMix != null)
            {
                Mix deleteEntity = (Mix)retrievedMix.Result;
                TableOperation delete = TableOperation.Delete(deleteEntity);
            }
        }
    }
}
