<template>
  <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      <button type="button" @click="loadSpotifyPlaylist()">Spotify</button>
      <button type="button" @click="loadDeezerPlaylist()">Deezer</button>
	    <div class="playlists">
	<div class="listplaylist" v-for="playlist in playlist">
    	{{ playlist.Name }}
    	<img class="imgplaylist" v-bind:src="playlist.Cover"/>
    </div>
  	</div>
  </div>
  </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import SpotifyApiService from '../services/SpotifyApiService'
    import DeezerApiService from '../services/DeezerApiService'

  export default {
        data() {
            return {
                playlist: []
            }
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadSpotifyPlaylist: async function() {
                var playlist = await this.requestAsync(() => SpotifyApiService.getSpotifyPlaylist());
                this.playlist = playlist;
            },
            loadDeezerPlaylist: async function() {
                var playlist = await this.requestAsync(() => DeezerApiService.getDeezerPlaylist());
                this.playlist = playlist;
            },

        }

    }

</script>

<style src="../styles/playlists.css">

</style>