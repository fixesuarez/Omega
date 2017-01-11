using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DAL
{
    public class PseudoIndex : TableEntity
    {
        public PseudoIndex() { }

        public string Guid { get; set; }
    }
}
