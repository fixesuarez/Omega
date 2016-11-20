<template>
<div id="Login">
    <div class="Cd">
      <img id="Cd" src="../assets/cd.png">
    <img id="Logo" src="../assets/Logo2-grey.png">
    </div>
  <div id="FbPanel">
    <button type="button" class="FbButton" @click="login('Facebook')"> Se connecter via Facebook</button>
  </div>   
  <div id="StreamPanel">
    <button type="button" class="DeButton" @click="login('Deezer')">Se connecter via Deezer</button>
    <button type="button" class="SpButton" @click="login('Spotify')">Se connecter via Spotify</button>
  </div>
</div>   
</template>

<script>
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
<style src="../styles/Home.css"">
</style>