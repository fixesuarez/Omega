<template>
      <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      groups: {{group}}
      <button type="button" @click="loadFacebookGroup()">Groups</button>
    </div>
  </div>

</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import FacebookApiService from '../services/FacebookApiService'
   

  export default {
        data() {
            return {
                group: []
            }
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadFacebookGroup: async function() {
                var group = await this.requestAsync(() => FacebookApiService.getFacebookGroup());
                this.group = group;
            }
        }

    }

</script>