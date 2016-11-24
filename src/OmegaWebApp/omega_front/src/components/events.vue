<template>
      <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      playlist: {{playlist}}
      <button type="button" @click="loadSpotifyPlaylist()">Spotify</button>
      <button type="button" @click="loadDeezerPlaylist()">Deezer</button>
    </div>
  </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import FacebookApiService from '../services/FacebookApiService'
   

  export default {
        data() {
            return {
                event: []
            }
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadFacebookGroup: async function() {
                var event = await this.requestAsync(() => FacebookApiService.getFacebookEvent());
                this.event = event;
            }
        }

    }

</script>