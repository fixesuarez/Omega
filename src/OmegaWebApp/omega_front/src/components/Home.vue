<template>
<div id="Login">
    <div class="Cd">
    <img id="Logo" src="../assets/Logo2-grey.png">
    </div>
  <div id="FbPanel">
    <button type="button" class="FbButton" @click="login('Facebook')"><img  src="../assets/facebookConnect.png"></button>
  </div>   
  <div id="StreamPanel">
    <button type="button" class="DeButton" @click="login('Deezer')"><img  src="../assets/deezerConnect.png"></button>
    <button type="button" class="SpButton" @click="login('Spotify')"><img  src="../assets/spotifyConnect.png"></button>
           <br> <router-link to="/playlists">Passer la connection</router-link>
  </div>
</div>   
</template>

<script>

import AuthService from '../services/AuthService'

export default {
  methods: {
    login(provider) {
    AuthService.login(provider);
    },
    onAuthenticated() {
    this.$router.replace('/');
    }
  }
}

</script>

<style src="../styles/Home.css">
</style>