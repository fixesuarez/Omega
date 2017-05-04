using NUnit.Framework;
using Omega.DAL;
using System.Threading.Tasks;

namespace Omega.FacebookCrawler.Tests
{
    [TestFixture]
    public class FacebookApiServiceTests
    {
        FacebookApiService _facebookApiService;

        [SetUp]
        public void init()
        {
            _facebookApiService = new FacebookApiService( new EventGroupGateway( "UseDevelopmentStorage=true" ), new UserGateway( "UseDevelopmentStorage=true" ) );
        }

        [Test]
        public async Task InsertGroups()
        {
            string email = "francoisxaviersuarez@gmail.com";
            await _facebookApiService.GetAllFacebookEvents( email );
        }
    }
}
