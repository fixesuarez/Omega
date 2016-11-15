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
export default {
  methods: {
    login(provider) {
      this.$http.get('http://localhost:5000/Account/login/' + provider)
    },
    onAuthenticated() {
          this.$router.replace('/');
  }
}
</script>
<style src="../styles/Home.css"">
</style>