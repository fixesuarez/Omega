using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class MixTableGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableMixTable;
        readonly CloudBlobClient _blobClient;
        readonly CloudBlobContainer _container;

        public MixTableGateway(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _tableClient = _storageAccount.CreateCloudTableClient();
            _blobClient = _storageAccount.CreateCloudBlobClient();
            _container = _blobClient.GetContainerReference("playlist-container");
            _tableMixTable = _tableClient.GetTableReference("MixTable");
            _tableMixTable.CreateIfNotExistsAsync().Wait();
            _container.CreateIfNotExistsAsync().Wait();
        }

        public async Task<Mix> RetrieveMix(string mixName, string guid)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Mix>(guid, mixName);
            TableResult retrievedMix = await _tableMixTable.ExecuteAsync(retrieveOperation);
            Mix mix = (Mix) retrievedMix.Result;
            return mix;
        }

        public async Task InsertMix(Mix mix, string guid)
        {
            if(await RetrieveMix(mix.PartitionKey, guid) == null)
            {
                CloudBlockBlob blockBlob = _container.GetBlockBlobReference(guid +":"+ mix.RowKey);
                await blockBlob.UploadTextAsync(mix.playlist);
                mix.playlist = guid + ":"+ mix.RowKey;
                
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

        public async Task<List<Mix>> RetrieveAllMixUser(string guid)
        {
            List<Mix> mixs = new List<Mix>();
            TableQuery<Mix> query = new TableQuery<Mix>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, guid));

            query.TakeCount = 1000;
            TableContinuationToken tableContinuationToken = null;
            do
            {
                var queryResponse = await _tableMixTable.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                tableContinuationToken = queryResponse.ContinuationToken;
                mixs.AddRange(queryResponse.Results);
            } while (tableContinuationToken != null);

            foreach(Mix mix in mixs)
            {
                CloudBlockBlob blockBlob = _container.GetBlockBlobReference(guid + ":" + mix.RowKey);
                string text;
                using (var memoryStream = new MemoryStream())
                {
                    await blockBlob.DownloadToStreamAsync(memoryStream);
                    text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                mix.playlist = text;
            }

            return mixs;
        }
    }
}
