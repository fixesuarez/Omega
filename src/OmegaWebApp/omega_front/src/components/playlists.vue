<template>
  <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
    <button type="button" @click="loadPlaylists()">Load all playlist</button>
    {{playlists}}
      <!--<button type="button" @click="loadSpotifyPlaylist()">Spotify</button>
      <button type="button" @click="loadDeezerPlaylist()">Deezer</button>
        <div class="listgroup" v-for="playlist in playlists">
      <div class="content-group" >
      <div class="content-group-image"> 
        <img class="imggroup" v-bind:src="playlist.Cover" /> </div>
        {{ playlist.Name }}
    </div>
  </div>
  	</div>-->
  </div>
  </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import SpotifyApiService from '../services/SpotifyApiService'
    import DeezerApiService from '../services/DeezerApiService'
    import PlaylistApiService from '../services/PlaylistApiService'

  export default {
        data() {
            return {
                //Splaylist: [],
                //Dplaylist: []
                playlists: []
            }
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadPlaylists: async function() {
                var playlists = await this.requestAsync(() => PlaylistApiService.getPlaylists());
                this.playlists = playlists;
            }

           /* loadSpotifyPlaylist: async function() {
                var playlists = await this.requestAsync(() => SpotifyApiService.getSpotifyPlaylist());
                this.playlists = playlists;
            },
            loadDeezerPlaylist: async function() {
                var dzplaylist = await this.requestAsync(() => DeezerApiService.getDeezerPlaylist());
                this.playlists.push(dzplaylist);
            },*/
        }
    }

</script>

<style src="../styles/playlists.css">

</style>