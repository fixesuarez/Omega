<template>
  <div class="global">
    <div class="firstPage">
      <div class="navbar">
        <a href="" @click="login('Facebook')">FACEBOOK</a>
        <a href="" id="orangeText" @click="login('Deezer')">DEEZER</a>
        <a href="" @click="login('Spotify')">SPOTIFY</a>
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
import { mapGetters, mapActions } from 'vuex'
import Vue from 'vue'
import $ from 'jquery'

export default {
  data() {
      return {
          endpoint: null
      }
  },
  methods: {
    login(provider) {
    AuthService.login(provider);
    },
    onAuthenticated() {
    this.$router.replace('/playlist');
    },
    Authverif: function() {
      if(this.identity == true){
            this.$router.replace('/playlist');
      }
    }
  },
  created() {
    AuthService.registerAuthenticatedCallback(this.onAuthenticated);
    this.Authverif();
  },
  beforeDestroy() {
    AuthService.removeAuthenticatedCallback(this.onAuthenticated);
  },
  computed: {
    ...mapGetters(['finalMix', 'identity', 'currentTrack','finalPlaylist'])
    
  },
}

</script>

<style src="../styles/home2.css">
</style>

