<template>
  <div class="col-12 playlistGlobal">
    <button type="button" @click="loadSpotifyPlaylist(),loadDeezerPlaylist()">Refresh Playlist</button>
    <button type="button" @click="loadPlaylists()">show Playlist</button>
    
    <!--<button @click="insertMood(mood)">Send mood</button>-->
    <button @click="startMix()">Mix</button>


    <!--Contains the top part of the playlist vue-->
    <div class="col-12 playlistContainer">  
      <div class="playlistWrapper" id="playlistWrapper">
        <img src="../assets/rightButton.png" class="rightScroll" @click="scrollRight">
        <img src="../assets/leftButton.png" class="leftScroll" @click="scrollLeft">
        <span v-for="p in playlists" @click="selectPlaylist(p),setSPlayer(),setDPlayer()" id="spanPlaylist">
          <img v-if="p.check == false" v-bind:src="p.Cover" class="playlistImage">
          <img v-else="p.check == true" v-bind:src="p.Cover" class="checkedImage">
          <span class="imageOverlay">
            <img src="https://cdn4.iconfinder.com/data/icons/flat-black/512/menu.png" id="settingsImage">
            {{p.Name}}
          </span>
          <span v-if="p.provider == 's'" class="sPlaylistBanner">
            SPOTIFY
          </span>
          <span v-if="p.provider == 'd'" class="dPlaylistBanner">
            DEEZER
          </span>
        </span>
      </div>
    </div>

    <!--Contains the bottom part of the playlist vue-->

    <div class="col-12 playlistNavbar">

      <!--Left-->

      <div class="col-7 leftBottom">
        <div class="playlistAndTracks">
          <div class="currentPlaylistImage">
            <img v-bind:src="currentPlaylist.Cover">
          </div>
          <div class="tracks">
            <span id="playlistTitle">{{currentPlaylist.Name}}</span><br>
            <span id="playlistOwner">{{currentPlaylist.OwnerId}}</span><br>
            <span id="tracksLength" v-if="currentPlaylist.Tracks != null">{{currentPlaylist.Tracks.length}} titres</span><br>
            <!--<div v-for="track in currentPlaylist.Tracks" class="track" @click="currentTrack == track.id">
              {{track.Title}}<br>
            </div>-->
            <iframe v-if="currentPlaylist.provider == 's'" v-bind:src="sPlayer" width="100%" height="75%" frameborder="0" allowtransparency="true"></iframe>
            <iframe v-if="currentPlaylist.provider == 'd'" scrolling="no" frameborder="0" allowTransparency="true" v-bind:src="DzPlayer" width="100%" height="300"></iframe>
          </div>
        </div>
      </div>

      <!--Right-->
      
      <div class="col-5 rightBottom">
        {{finalMix }}
      </div>
    </div>

    <PlaylistHelperModal v-if="playlistHelperModalActive == true"></PlaylistHelperModal>
    <EventModal v-if="eventModalActive == true"></EventModal>
    <MoodsModal v-if="moodsModalActive == true"><MoodsModal>
  </div>
</template> 

<script>
import { mapGetters, mapActions } from 'vuex'
import PlaylistHelperModal from '../components/playlistHelperModal.vue'
import EventModal from '../components/events.vue'
import MoodsModal from '../components/moods.vue'
import PlaylistApiService from '../services/PlaylistApiService'
import SpotifyApiService from '../services/SpotifyApiService'
import DeezerApiService from '../services/DeezerApiService'
import MoodService from '../services/MoodService'
import MixService from '../services/MixService'
import AuthService from '../services/AuthService'

export default {
  data () {
    return {
      finalMix: '',
      sPlayer: '',
      DzPlayer: '',
      sPlaylists: [],
      Connected: false,
      dPlaylists: [],
      SDplaylists: [],
      currentTrack: '',
      number: 1,
      height: '',
      localPlaylists: [{"id":0,"name":"Apero Party","image":"http://d817ypd61vbww.cloudfront.net/sites/default/files/styles/media_responsive_widest/public/tile/image/A-116-01comp.jpg?itok=LjWmwzjU"},{"id":1,"name":"Mexico","image":"https://caliloved.files.wordpress.com/2013/07/deer-album-cover-new.jpg"},{"id":2,"name":"Chanson francaise","image":"http://www.designformusic.com/wp-content/uploads/2016/01/perfectly-chilled-album-cover-artwork-design-500x500.jpg"},{"id":3,"name":"Cam box","image":"https://www.smashingmagazine.com/images/music-cd-covers/64.jpg"},{"id":4,"name":"Jaccuzi money billey","image":"https://img.buzzfeed.com/buzzfeed-static/static/2016-01/27/11/enhanced/webdr14/enhanced-6784-1453912540-22.jpg"},{"id":5,"name":"Beer-Pong","image":"http://androidjones.com/wp-content/uploads/2012/05/HOPE-1024x1024.jpg"},{"id":6,"name":"Runing Time","image":"http://illusion.scene360.com/wp-content/uploads/2014/10/computergraphics-album-covers-2014-05.jpg"},{"id":7,"name":"Soiree OKLM","image":"http://www.fuse.tv/image/5682ea90ac0e76bd68000055/768/512/brown-eyed-girls-basic-album-cover-full-size.jpg"},{"id":8,"name":"Apero Party","image":"http://takuya.fr/wp-content/uploads/2016/06/takuya-fr-thedoubt.jpg"}],
      mood: {'cover': 'http://www.firstredeemer.org/wp-content/uploads/girl-backpack-thinking-sunset-field-fence-.jpg', 'name': 'Heyyy', 'metadonnees': {'Accousticness': '0.45', 'Danceability': '0.22', 'Energy': '0.84', 'Instrumentalness': '0.44', 'Liveness': '0.11', 'Loudness': '-44', 'Mode': '1', 'Popularity': '28'}}
    }
  },
  methods: {
    ...mapActions(['checkPlaylist', 'setCurrentPlaylist', 'selectPlaylist', 'sendPlaylists', 'requestAsync', 'inserteMood', 'mix', 'getIdentity']),
    setSPlayer: function() {
      var player = 'https://embed.spotify.com/?uri=spotify:user:'+ this.currentPlaylist.OwnerId +':playlist:'+ this.currentPlaylist.PlaylistId;
      this.sPlayer = player;
    },
    setDPlayer: function() {
     var player = 'https://www.deezer.com/plugins/player?format=classic&autoplay=false&playlist=true&width=350&height=350&color=007FEB&layout=dark&size=medium&type=playlist&id='+ this.currentPlaylist.PlaylistId +'&app_id=176241';
      this.DzPlayer = player;
    },
    scrollRight: function() {
      var scroll = document.getElementById('spanPlaylist').offsetWidth;
      document.getElementById('playlistWrapper').scrollLeft += scroll;
    },
    scrollLeft: function() {
      var scroll = document.getElementById('spanPlaylist').offsetWidth;
      document.getElementById('playlistWrapper').scrollLeft -= scroll;
    },
    insertMood: async function() {
      this.$http.post('http://localhost:5000/api/Ambiance/InsertAmbiance', this.mood, function () {
       })
    },
    loadPlaylists: async function() {
      this.SDplaylists = await this.requestAsync(() => PlaylistApiService.getPlaylists());
      this.SDplaylists.map(m => { this.$set(m, 'check', false); return m});
      this.sendPlaylists(this.SDplaylists);
    },
    loadSpotifyPlaylist: async function() {
      var spplaylist = await this.requestAsync(() => SpotifyApiService.getSpotifyPlaylist());
      this.sPlaylists.push(spplaylist);
      this.height = 'reussi';
    },
    loadDeezerPlaylist: async function() {
      var dzplaylist = await this.requestAsync(() => DeezerApiService.getDeezerPlaylist());
      this.dPlaylists.push(dzplaylist);
    },
    startMix: async function() {
      this.mix();
      this.finalMix = await MixService.mix(this.mixToMix);
      // this.$http.post('http://localhost:5000/api/Mix/MixPlaylist', this.mixToMix, function () {
      //  })
    },
    increment: function() {
      this.number++;
    }
  },
  computed: {
    ...mapGetters(['playlistHelperModalActive', 'eventModalActive', 'moodsModalActive', 'playlists', 'currentPlaylist', 'currentMood', 'checkedPlaylists', 'moodToInsert', 'mixToMix', 'identity'])
  },
  created () {
    if(this.playlists.length === 0) {
      this.loadPlaylists();
    }
    this.getIdentity(true);
  },
  mounted () {
  },
  components: {
    PlaylistHelperModal,
    EventModal,
    MoodsModal
  }
}
</script>

<style>
/*Set the element size and position*/
.playlistGlobal {
  height: 72vh;
}

.playlistContainer {
  height: 26vh;
  min-height: 169px;
  background: #0e1014;
}

.playlistNavbar {
  height: 46vh;
  min-height: 299px;
  background: #171c27;
}

/*Playlists display*/
.playlistWrapper {
  height: 100%;
  width: 100%;
  white-space: nowrap;
  overflow-x: auto;
}

#spanPlaylist {
  position: relative;
  transition: all .5s ease;
  color: white;
}

.playlistImage {
  width: 200px;
  margin-left: 10px;
  margin-right: 10px;
  opacity: 0.5;
  box-shadow: 0px 0px 24px 1px rgba(0,0,0,1);
  transition: all .5s ease;
}

#spanPlaylist:hover > .imageOverlay {
  opacity: 1;
}

.checkedImage {
  width: 200px;
  margin-left: 10px;
  margin-right: 10px;
  box-shadow: 0px 0px 24px 1px rgba(0,0,0,1);
  transition: all .5s ease;
}

.imageOverlay {
  position: absolute;
  width: 200px;
  height: 200px;
  left: 10px;
  background: linear-gradient(to bottom, rgba(0, 0, 0, 1), rgba(0, 0, 0, 0));
  opacity: 0;
  transition: all .5s ease;
  font-family: 'Montserrat-ultra-light';
  font-size: 14px;
  text-align: center;
  overflow: hidden;
  text-transform: uppercase;
  text-overflow: ellipsis;
}

#settingsImage {
  width: 30px;
  filter: invert(1);
  vertical-align: middle;
}

.sPlaylistBanner {
  position: absolute;
  margin-top: 160px;
  left: 5px;
  height: 25px;
  background: green;
  border-radius: 0px 0px 2px 2px;
  padding: 2px;
  font-size: 13px;
  font-family: 'Montserrat-ultra-light';
}

.dPlaylistBanner {
  position: absolute;
  margin-top: 160px;
  left: 5px;
  height: 25px;
  background: green;
  border-radius: 0px 0px 2px 2px;
  padding: 2px;
  font-size: 13px;
  font-family: 'Montserrat-ultra-light';
}

.playlistSettings:hover {
  background: blue;
  height: 100px;
}

.rightScroll {
  position: absolute;
  width: 18px;
  opacity: 0.5;
  z-index: 2;
  right: 0;  
  margin-right: 10px;
  margin-top: 4%;
  transition: all .3s ease;
}

.leftScroll {
  position: absolute;
  width: 18px;
  opacity: 0.5;
  z-index: 2;
  left: 0;
  margin-left: 10px;
  margin-top: 4%;
  transition: all .3s ease;
}

.leftScroll:hover, .rightScroll:hover {
  width: 22px;
  opacity: 0.7;
}

/*Left bottom panel display*/

.leftBottom {
  padding: 10px;
  height: 100%;
}

.playlistAndTracks {
  height: 100%;
  color: white;
  float: right;
}

.currentPlaylistImage {
  height: 100%;
  float: left;
}

.currentPlaylistImage img {
  height: 100%;
}

.tracks {
  min-width: 200px;
  max-width: 300px;
  height: 100%;
  float: left;
  overflow-y: auto;
  overflow-x: hidden;
  text-overflow: ellipsis;
}

.track {
  font-size: 12px;
  font-family: 'Montserrat-ultra-light';
  padding-left: 10px;
  padding-top: 10px;
  white-space: nowrap;
  height: 35px;
}

#playlistTitle {
  font-family: 'Montserrat-ultra-light';
  font-size: 24px;
  text-transform: uppercase;
  margin-left: 10px;
}

#playlistOwner {
  font-family: 'Montserrat-regular';
  font-size: 14px;
  line-height: 30px;
  margin-left: 10px;
}

#tracksLength {
  font-family: 'Montserrat-ultra-light';
  line-height: 40px;
  font-size: 14px;
  margin-left: 10px;
}

.playlistCover-enter-active, .playlistCover-leave-active {
  transition: opacity .5s
}

.playlistCover-enter, .playlistCover-leave-active {
  opacity: 0
}

/*Right bottom panel display*/

.rightBottom {
  height: 100%;
  background: #12161e;
}

@media screen and (max-height: 900px) {
  .playlistImage, .checkedImage {
    width: 180px;
  }
}

@media screen and (max-height: 800px) {
  .playlistImage, .checkedImage {
    width: 160px;
  }
}

@media screen and (max-height: 700px) {
  .playlistImage, .checkedImage {
    width: 140px;
  }
}

</style>