<template>
<div id="Login">
    <div class="Cd">
    <img id="Logo" src="../assets/Logo2-grey.png">
    </div>
  <div id="FbPanel">
    <span @click="authenticate(true)"><button type="button" class="FbButton" @click="login('Facebook')"> Se connecter via Facebook</button></span>
  </div>   
  <div id="StreamPanel">
    <span @click="authenticate(true)"><button type="button" class="DeButton" @click="login('Deezer')">Se connecter via Deezer</button></span>
    <span @click="authenticate(true)"><button type="button" class="SpButton" @click="login('Spotify')">Se connecter via Spotify</button></span>
            <span @click="authenticate(true)"><router-link to="/playlists">Passer la connection</router-link><span>
  </div>
</div>   
</template>

<script>

import AuthService from '../services/AuthService'
import { mapGetters, mapActions } from 'vuex'

export default {
  methods: {
    login(provider) {
    AuthService.login(provider);
    },
    onAuthenticated() {
    this.$router.replace('/');
    },
    ...mapActions(['authenticate'])
  }
}

</script>

<style src="../styles/Home.css">
</style>