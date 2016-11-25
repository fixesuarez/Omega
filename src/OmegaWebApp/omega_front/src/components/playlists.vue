<template>
  <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      playlist : {{playlists}}
      <button type="button" @click="loadSpotifyPlaylist()">Spotify</button>
      <button type="button" @click="loadDeezerPlaylist()">Deezer</button>
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
                //Splaylist: [],
                //Dplaylist: []
                playlists: []
            }
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadSpotifyPlaylist: async function() {
                var playlists = await this.requestAsync(() => SpotifyApiService.getSpotifyPlaylist());
                this.playlists = playlists;
                loadDeezerPlaylist();
            },
            loadDeezerPlaylist: async function() {
                var dzplaylist = await this.requestAsync(() => DeezerApiService.getDeezerPlaylist());
                this.playlists.push(dzplaylist);
            },

        }

    }

</script>

<style src="../styles/playlists.css">

</style>