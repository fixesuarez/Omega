<template>
  <div class="global">
    <div class="firstPage">
      <div class="navbar">
        <a href="" @click="login('Facebook')">FACEBOOK</a>
        <a href="" id="orangeText" @click="login('Deezer')">DEEZER</a>
        <a href="" @click="login('Spotify')">SPOTIFY</a>
        <router-link to="/playlist">SKIP</router-link>
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
    }
  },
  created() {
    AuthService.registerAuthenticatedCallback(this.onAuthenticated);
  },
  beforeDestroy() {
    AuthService.removeAuthenticatedCallback(this.onAuthenticated);
  },  
}

</script>

<style src="../styles/home2.css">
</style>

