using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DAL
{
    public class PseudoIndex : TableEntity
    {
        public PseudoIndex() { }
        public PseudoIndex( string pseudo, string guid )
        {
            PartitionKey = string.Empty;
            RowKey = pseudo;
            Guid = guid;
        }
        public string Guid { get; set; }
    }
}
