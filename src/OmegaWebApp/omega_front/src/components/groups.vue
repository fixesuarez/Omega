<template>
      <div class="wrapper"> <!-- TAB TAB APRES AVOIR TAPER LE MOT POUR FAIRE UNE BALISE -->
    <div class="playlistsPanel">
      groups: {{groups}}
      <button type="button" @click="loadFacebookGroup()">Groups</button>
  <div class="listgroup" v-for="group in groups">
      <div class="content-group" >
      <div class="content-group-image"> 
        <img class="imggroup" v-bind:src="group.Cover" /> </div>
        {{ group.Name }}
    </div>
  </div>
    </div>
  </div>

</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import FacebookApiService from '../services/FacebookApiService'
   

  export default {
        data() {
            return {
                groups:[]
            }
        },
        methods: {
            ...mapActions(['requestAsync']),

            loadFacebookGroup: async function() {
                var groups = await this.requestAsync(() => FacebookApiService.getFacebookGroup());
                this.groups = groups;
            }
        }
    }

</script>

<style>
    .listgroup {
  margin-left: 1%;
  padding-top: 0%;
  margin-left: 0.5%;
  display: inline-block;
  width: 150px;
  height: 220px;
  border-style:none;
  margin-bottom: -2.6%;
}

.imggroup {

  width:150px;
  height:150px;
}
</style>