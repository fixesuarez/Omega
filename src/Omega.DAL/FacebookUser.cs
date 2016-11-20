using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DAL
{
    public class FacebookUser : TableEntity
    {
        public string Email { get; set; }

        public FacebookUser() { }

        public FacebookUser( string facebookId, string email )
        {
            PartitionKey = string.Empty;
            RowKey = facebookId;
            Email = email;
        }
    }
}
