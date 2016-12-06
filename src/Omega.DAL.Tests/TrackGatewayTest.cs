using NUnit.Framework;
using System.Threading.Tasks;

namespace Omega.DAL.Tests
{
    [TestFixture]
    public class TrackGatewayTest
    {
        TrackGateway _trackGateway;

        [SetUp]
        public void Init()
        {
            _trackGateway = new TrackGateway( "UseDevelopmentStorage=true" );
        }
        [TearDown]
        public async Task End()
        {
            await _trackGateway.DeleteTrack( "playlistId", "test", "trackId" );
        }

        [Test]
        public async Task Insert_And_Retrieve_Tracks_Work_Correclty()
        {
            await _trackGateway.InsertTrack( "test", "playlistId", "trackId", "title", "albumName", "popularity", "duration", "image" );
            Track t = await _trackGateway.RetrieveTrack( "test", "playlistId", "trackId" );

            Assert.AreEqual( "playlistId", t.PartitionKey );
            Assert.AreEqual( "trackId", t.TrackId );
            Assert.AreEqual( "title", t.Title );
            Assert.AreEqual( "albumName", t.AlbumName );
            Assert.AreEqual( "popularity", t.Popularity );
            Assert.AreEqual( "duration", t.Duration );
            Assert.AreEqual( "image", t.Cover );
        }
    }
}
