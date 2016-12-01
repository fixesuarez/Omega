<template>
      <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      events: {{event}}
      <button type="button" @click="loadFacebookEvent()">Event</button>
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

            loadFacebookEvent: async function() {
                var event = await this.requestAsync(() => FacebookApiService.getFacebookEvent());
                this.event = event;
            }
        }

    }

</script>