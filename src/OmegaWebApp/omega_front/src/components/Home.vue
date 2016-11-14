<template>
  <div id="app">
    <html>
      <body>
               <div class="text-center">
            <div class="page-header">
                <h1>Bienvenue sur Omega</h1>
            </div>

            <button type="button" class="FbButton" @click="login('Facebook')"> Se connecter via Facebook</button>
            <button type="button" class="DeButton" @click="login('Deezer')"></button>
            <button type="button" class="SpButton" @click="login('Spotify')">Se connecter via Spotify</button>
        </div>
      </body>
    </html>
  </div>    
</template>

<script>
export default {
  methods: {
    login: function (provider) {
      this.$http.get('http://omega.itinet.fr/controller/login/' + provider  
    }
  }
}
</script>
<style src="../styles/Home.css">
</style>