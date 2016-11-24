<template>
  <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      Splaylist: {{Splaylist}}
            Dplaylist: {{Dplaylist}}
      <button type="button" @click="loadSpotifyPlaylist()">Spotify</button>
      <button type="button" @click="loadDeezerPlaylist()">Deezer</button>
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
                Splaylist: [],
                Dplaylist: []
            }
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadSpotifyPlaylist: async function() {
                var Splaylist = await this.requestAsync(() => SpotifyApiService.getSpotifyPlaylist());
                this.Splaylist = Splaylist;
            },
            loadDeezerPlaylist: async function() {
                var Dplaylist = await this.requestAsync(() => DeezerApiService.getDeezerPlaylist());
                this.Dplaylist = Dplaylist;
            },

        }

    }

</script>

<style src="../styles/playlists.css">
</style>