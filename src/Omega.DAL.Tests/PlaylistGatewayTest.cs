using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DAL.Tests
{
    [TestFixture]
    public class PlaylistGatewayTest
    {
        //PlaylistGateway _playlistGateway;
        //TrackGateway _trackGateway;

        //[SetUp]
        //public void Init()
        //{
        //    _playlistGateway = new PlaylistGateway( "UseDevelopmentStorage=true" );
        //    _trackGateway = new TrackGateway( "UseDevelopmentStorage=true" );
        //}
        //[TearDown]
        //public async Task End()
        //{
        //    await _trackGateway.DeleteTrack( "playlistId", "test", "trackId" );
        //    await _trackGateway.DeleteTrack( "playlistId", "test", "trackId1" );
        //}
        //[Test]
        //public async Task InsertPlaylist_And_RetrieveAllTracksFromPlaylist_Work_Correctly()
        //{
        //    List<Track> tracks = new List<Track>();
        //    await _trackGateway.InsertTrack( "test", "playlistId", "trackId", "title", "albumName", "popularity", "duration", "image" );
        //    await _trackGateway.InsertTrack( "test", "playlistId", "trackId1", "title1", "albumName1", "popularity1", "duration1", "image1" );
        //    tracks.Add( await _trackGateway.RetrieveTrack( "test", "playlistId", "trackId" ) );
        //    tracks.Add( await _trackGateway.RetrieveTrack( "test", "playlistId", "trackId1" ) );

        //    Playlist p = new Playlist( "fx", "playlistId", tracks, "playlistName", "playlistCover" );
        //    await _playlistGateway.InsertPlaylist( p );

        //    List<Track> retrievedTracksFromPlaylist = await _playlistGateway.RetrieveTracksFromPlaylists( p );
        //}
    }
}
