<template>
  <div class="global">
    <div class="firstPage">
      <div class="navbar">
        <span @click="login('Facebook')">FACEBOOK</span>
        <span id="orangeText" @click="login('Deezer')">DEEZER</span>
        <span @click="login('Spotify')">SPOTIFY</span>
      </div>
      <div class="header">
        <img src="../assets/triangleGrey.png" id="logo"><br>
        <div class="title"><span class="redText">o</span>mega</div>
      </div>
    </div>

    <div class="secondPage">
      test
    </div>
  </div>
</template>

<script>
import AuthService from '../services/AuthService'
import PseudoService from '../services/PseudoService'
import { mapGetters, mapActions } from 'vuex'
import Vue from 'vue'
import $ from 'jquery'

export default {
  data() {
      return {
          endpoint: null,
          provider: ''
      }
  },
  methods: {
        ...mapActions(['sendPseudo', 'getIdentity','requestAsync', 'setConnection']),
    login(provider) {
    this.provider = provider;
    AuthService.login(provider);
    },
    onAuthenticated() {
    this.$router.replace('/playlist');
    this.setConnection(this.provider);
    },
    Authverif: function() {
      if(this.pseudo != ''){
      this.$router.replace('/playlist');
      }
      console.log(this.pseudo)
    },
    loadPseudo: async function() {
      var pseudo = await this.requestAsync(() => PseudoService.getPseudo());   
    this.sendPseudo(pseudo.Pseudo);
    this.Authverif();
    },
  },
  created() {
    AuthService.registerAuthenticatedCallback(this.onAuthenticated);
  },
  beforeDestroy() {
    AuthService.removeAuthenticatedCallback(this.onAuthenticated);
  },
  computed: {
    ...mapGetters(['finalMix', 'identity', 'currentTrack','finalPlaylist','pseudo'])
  },
  mounted() {
    this.loadPseudo();
  }
}

</script>

<style src="../styles/home2.css">
</style>

