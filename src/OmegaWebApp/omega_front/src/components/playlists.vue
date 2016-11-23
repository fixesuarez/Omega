<template>
  <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      playlist: 
      <button type="button" @click="loadPlaylist()">test</button>
                      <tr v-for="i of playlist">
                    <td>{{ i.OwnerId }}</td>
                    <td>{{ i.PlaylistId }}</td>
                    <td>{{ i.RowKey }}</td>
                </tr>
    </div>
  </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import SpotifyApiService from '../services/SpotifyApiService'

  export default {
        data() {
            return {
                playlist: []
            }
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadPlaylist: async function() {
                var playlist = await this.requestAsync(() => SpotifyApiService.getPlaylistList());

                this.playlist = playlist;
            }
        }

    }

</script>

<style src="../styles/playlists.css">
</style>