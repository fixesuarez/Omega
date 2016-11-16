<template>
<div>
      <h1>Bienvenue sur Omega</h1>
  <div class="FbPanel">
    <button type="button" class="FbButton" @click="login('Facebook')"> Se connecter via Facebook</button>

  </div>   
  <div class="StreamPanel">
    <button type="button" class="DeButton" @click="login('Deezer')">Se connecter via Deezer</button>
    <button type="button" class="SpButton" @click="login('Spotify')">Se connecter via Spotify</button>
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