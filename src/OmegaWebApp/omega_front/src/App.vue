<template>
  <div>
    <div v-if="identity == true" class="col-12 navbarContainer">
      <div class="col-12 appNavbar">
        <div class="col-4">
          <img src="./assets/triangleGrey.png" id="appLogo"><span class="appTitle"><span id="appRedText">o</span>mega</span>
        </div>
        <div class="col-4 appProviders">

          <span @click="relogin('Facebook')">FACEBOOK</span>
          <span @click="relogin('Deezer')" id="appRedText">DEEZER</span>
          <span @click="relogin('Spotify')">SPOTIFY</span>
        </div>
        <div class="col-4 appProfile">
          <img src="./assets/profile.png" id="appProfile"><span class="appProfileText">RODOLPHE WACHTER</span>
        </div>
      </div>
      <div class="col-12 appControlPanel">
        <div class="col-4">&nbsp</div>
        <div class="col-1"><span @click="showPlaylistHelperModal(true)"><img src="./assets/playlistsIcon.png"><br>playlists</span></div>
        <div class="col-1"><span @click="showEventModal(true)"><img src="./assets/eventIcon.png"><br>évènements</span></div>
        <div class="col-1"><span><img src="./assets/groupIcon.png"><br>groupes</span></div>
        <div class="col-1"><span @click="showMoodsModal(true), loadMoods()"><img src="./assets/moodIcon.png"><br>ambiances</div>
        <div class="col-4">&nbsp</div>
      </div>
    </div>
    <router-view></router-view>
  </div>
</template>

<script>
  import Login from './components/Login.vue'
  import playlists from './components/playlists.vue'
  import moods from './components/moods.vue'
  import Modal from './components/modal.vue'
  import { mapGetters, mapActions } from 'vuex'
  import AuthService from './services/AuthService'
  import Vue from 'vue'
  import $ from 'jquery'

  export default {
  data () {
    return {
      endpoint: null,
      active: 'playlistsTab',
      isActive: true,
      true: true,
      Connected: false,
      check: false,
      label: '',
      image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png',
      localCriterias: [
        { label: 'energy', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'popularity', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'instrumentalness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'speechiness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'acousticness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'danceability', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'tempo', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'valence', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'duration_ms', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" }
      ],
    }
  },
  created() {
    AuthService.registerAuthenticatedCallback(this.onAuthenticated);
    this.authVerify();
  },
  beforeDestroy() {
    AuthService.removeAuthenticatedCallback(this.onAuthenticated);
  },
  methods: {
    ...mapActions(['showPlaylistHelperModal', 'showEventModal', 'showMoodsModal', 'getIdentity', 'requestAsync', 'sendMoods']),
    login(provider) {
    AuthService.login(provider);
    },
    async relogin(provider) {
      await AuthService.relogin(provider);
    },
    onAuthenticated() {
    this.$router.replace('/playlist');
    },
    authVerify : function ()  {
      if (AuthService.identity != null) {
          this.Connected = true;
          this.getIdentity(this.Connected);
      }
    },
    loadMoods: async function() {
      var data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(data);
    }
  },
  computed: {
    ...mapGetters(['active', 'playlistHelperModalActive', 'moods', 'test', 'enabledCriterias', 'criterias', 'authenticated', 'identity'])
  },
  name: 'app',
  components: {
    Login,
    Modal,
    playlists
  },
  mounted () {
  }
}
</script>

<style src="./styles/app.css">
</style>