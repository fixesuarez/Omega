<template>
  <transition name="playlists">
  <div class="col-12 pContainer">
    <div class="playlistsWrapper">
      <div class="col-12 pPlaylists" id="pPlaylists">
        <img src="../assets/rightButton.png" class="rightButton" @click="scrollRight">
        <img src="../assets/leftButton.png" class="leftButton" @click="scrollLeft">
        <span v-for="p in playlists" @click="selectPlaylist(p)" id="spanPlaylist">
          <img v-if="p.check == false" v-bind:src="p.image" class="playlistImage">
          <img v-else="p.check == true" v-bind:src="p.image" class="checked">
        </span>
      </div>
    </div>
    <div class="col-12 pBottomPanel" id="pBottomPanel">
      <div class="col-1 pCurrentPlaylist">
      </div>
      <div class="col-6 pCurrentPlaylist">
        <div class="col-4 checkedPlaylists">
          <span class="col-12 checkedCounter">
            {{checkedPlaylists.length}} PLAYLISTS
          </span>
          <div class="col-3">&nbsp</div>
          <div class="col-6 checkedList">
            <span v-for="p in checkedPlaylists">
              <img v-bind:src="p.image" @click="setCurrentPlaylist(p)">
            </span>
          </div>
          <div class="col-3">&nbsp</div>
        </div>
        <div class="col-4">
          <img v-bind:src="currentPlaylist.image" class="currentPlaylistImage">
        </div>
        <div class="col-4 playlistDetails">
          <span id="playlistName">{{currentPlaylist.name}}</span><br>
          <span id="playlistAuthor">{{currentPlaylist.id}}</span><br>
          <span id="tracksNumber">{{currentPlaylist.id}} titres</span><br>
          <span id="playlistDuration">{{currentPlaylist.id}} minutes</span>
        </div>
      </div>
      </div>
      <div class="col-5 pCurrentTrackList">
        &nbsp
      </div>
      <modal v-if="modalActive == true"></modal>
    </div>
  </div>
  </transition>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import Modal from '../components/playlistHelperModal.vue'
import $ from 'jquery'

export default {
  data () {
    return {
      height: '',
      localPlaylists: [{"id":0,"name":"Apero Party","image":"http://d817ypd61vbww.cloudfront.net/sites/default/files/styles/media_responsive_widest/public/tile/image/A-116-01comp.jpg?itok=LjWmwzjU"},{"id":1,"name":"Mexico","image":"https://caliloved.files.wordpress.com/2013/07/deer-album-cover-new.jpg"},{"id":2,"name":"Chanson francaise","image":"http://www.designformusic.com/wp-content/uploads/2016/01/perfectly-chilled-album-cover-artwork-design-500x500.jpg"},{"id":3,"name":"Cam box","image":"https://www.smashingmagazine.com/images/music-cd-covers/64.jpg"},{"id":4,"name":"Jaccuzi money billey","image":"https://img.buzzfeed.com/buzzfeed-static/static/2016-01/27/11/enhanced/webdr14/enhanced-6784-1453912540-22.jpg"},{"id":5,"name":"Beer-Pong","image":"http://androidjones.com/wp-content/uploads/2012/05/HOPE-1024x1024.jpg"},{"id":6,"name":"Runing Time","image":"http://illusion.scene360.com/wp-content/uploads/2014/10/computergraphics-album-covers-2014-05.jpg"},{"id":7,"name":"Soiree OKLM","image":"http://www.fuse.tv/image/5682ea90ac0e76bd68000055/768/512/brown-eyed-girls-basic-album-cover-full-size.jpg"},{"id":8,"name":"Apero Party","image":"http://takuya.fr/wp-content/uploads/2016/06/takuya-fr-thedoubt.jpg"}],
      test: ''
    }
  },
  methods: {
    ...mapActions(['checkPlaylist', 'setCurrentPlaylist', 'selectPlaylist', 'sendPlaylists', 'requestAsync']),

    scrollRight: function() {
      var scroll = document.getElementById('spanPlaylist').offsetWidth;
      document.getElementById('pPlaylists').scrollLeft += scroll;
    },
    scrollLeft: function() {
      var scroll = document.getElementById('spanPlaylist').offsetWidth;
      document.getElementById('pPlaylists').scrollLeft -= scroll;
    },
    loadPlaylists: async function() {
      var playlists = await this.requestAsync(() => PlaylistApiService.getPlaylists());
      this.playlists = playlists;
    },
    loadSpotifyPlaylist: async function() {
      var spplaylist = await this.requestAsync(() => SpotifyApiService.getSpotifyPlaylist());
      this.SDplaylists.push(spplaylist);
    },
    loadDeezerPlaylist: async function() {
      var dzplaylist = await this.requestAsync(() => DeezerApiService.getDeezerPlaylist());
      this.SDplaylist.push(dzplaylist);
    }
  },
  computed: {
    ...mapGetters(['modalActive', 'playlists', 'currentPlaylist', 'checkedPlaylists'])
  },
  created () { 
    if(this.playlists.length === 0) {
      this.sendPlaylists(this.localPlaylists.map(m => { this.$set(m, 'check', false); return m}));
    }
  },
  mounted () {
  },
  components: {
    Modal
  }
}
</script>

<style src="../styles/playlists.css">
</style>