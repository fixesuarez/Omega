using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DAL
{
    public class UserIndex : TableEntity
    {
        public string Guid { get; set; }

        public UserIndex() { }
    }
}
