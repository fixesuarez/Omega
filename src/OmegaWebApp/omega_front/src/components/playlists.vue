<template>
  <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      playlist: {{playlists}}
    </div>
  </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import SpotifyApiService from '../../services/SpotifyApiService'

  export default {
        data() {
            return {
                playlists: []
            }
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadPlaylist: async function() {
                var playlist = await this.requestAsync(() => SpotifyApiService.getPlaylistList());

                this.playlist = playlist;
            }
        },
        created: {
          this.loadPlaylist()
        }

    }

</script>

<style src="../styles/playlists.css">
</style>