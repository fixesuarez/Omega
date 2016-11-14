<template>
  <div id="app">
    <html>
      <body>
        <div class="leftContent">
          <div class="col-12">
            <div class="logo">
              <img id="imgLogo" src="../assets/Logo2-grey.png">
            </div>
          </div>
          <div class="col-12">
            <div class="profile">
              <img id="imgProfile" src="../assets/profile.png"><!--{{user.firstName}} {{user.Name}}-->rodolphe <span id="redText">wachter</span>
            </div>
          </div>
          <div class="col-12">
            <nav v-bind:class="active" v-on:click.prevent>
              <a href="#" class="playlistsTab" @click="makeActive('playlistsTab')">playlists</a><br>
              <a href="#" class="evenementsTab" @click="makeActive('evenementsTab')">evenements</a><br>
              <a href="#" class="groupesTab" @click="makeActive('groupesTab')">groupes</a><br>
              <!--<input type="radio" name="caca" v-bind:value="playlistsTab" v-on:click="testMakeActive" v-model="active">playlists<br>
              <input type="radio" name="caca" v-bind:value="evenementsTab" v-on:click="testMakeActive" v-model="active">evenements<br>
              <input type="radio" name="caca" v-bind:value="groupesTab" v-on:click="testMakeActive" v-model="active">groupes<br>-->
              <a href="#" class="ambiancesTab" id="redText" v-on:click="makeModalActive()">ambiances</a>
            </nav>
          </div>
          <div class="col-12">
            <div class="players">
              <span v-if="activePlayer === 'deezerPlayer'">
                <iframe class="spotifyPlayer" style="opacity: 0.2" src="https://embed.spotify.com/?uri=spotify:track:2V65y3PX4DkRhy1djlxd9p" width="250" height="80" frameborder="0" allowtransparency="true"></iframe>
                <iframe class="deezerPlayer" style="opacity: 1" scrolling="no" frameborder="0" allowTransparency="true" src="https://www.deezer.com/plugins/player?format=classic&autoplay=false&playlist=false&width=250&height=200&color=007FEB&layout=dark&size=medium&type=playlist&id=30595446&app_id=1" width="250" height="62"></iframe> {{activePlayer}} <button v-on:click="makeActivePlayer('spotifyPlayer')">Spotify</button><button v-on:click="makeActivePlayer('deezerPlayer')">Deezer</button>
              </span>
              <span v-if="activePlayer === 'spotifyPlayer'">
                <iframe class="spotifyPlayer" style="opacity: 1" src="https://embed.spotify.com/?uri=spotify:track:2V65y3PX4DkRhy1djlxd9p" width="250" height="80" frameborder="0" allowtransparency="true"></iframe>
                <iframe class="deezerPlayer" style="opacity: 0.2" scrolling="no" frameborder="0" allowTransparency="true" src="https://www.deezer.com/plugins/player?format=classic&autoplay=false&playlist=false&width=250&height=200&color=007FEB&layout=dark&size=medium&type=playlist&id=30595446&app_id=1" width="250" height="62"></iframe> {{activePlayer}} <button v-on:click="makeActivePlayer('spotifyPlayer')">Spotify</button><button v-on:click="makeActivePlayer('deezerPlayer')">Deezer</button>
              </span>
              <span v-if="activePlayer === ''">
                <iframe class="spotifyPlayer" style="opacity: 1" src="https://embed.spotify.com/?uri=spotify:track:2V65y3PX4DkRhy1djlxd9p" width="250" height="80" frameborder="0" allowtransparency="true"></iframe>
                <iframe class="deezerPlayer" style="opacity: 1" scrolling="no" frameborder="0" allowTransparency="true" src="https://www.deezer.com/plugins/player?format=classic&autoplay=false&playlist=false&width=250&height=200&color=007FEB&layout=dark&size=medium&type=playlist&id=30595446&app_id=1" width="250" height="62"></iframe> {{activePlayer}} <button v-on:click="makeActivePlayer('spotifyPlayer')">Spotify</button><button v-on:click="makeActivePlayer('deezerPlayer')">Deezer</button>
              </span>              
            </div>
          </div>
        </div>
        <div class="redLine">
        </div>
        <div class="modalMask" v-if="modalActive === true">
        </div>
        <div class="modalPanel" v-if="modalActive === true" @keyup.esc="makeModalActive()">
          <button v-on:click="makeModalActive()">Fermer</button>
        </div>
      </body>
    </html>
  </div>
</template>

<script>
export default {
  data () {
    return {
      active: 'playlistsTab',
      activePlayer: '',
      modalActive: false,
      playlistsTab: 'playlistsTab',
      evenementsTab: 'evenementsTab',
      groupesTab: 'groupesTab'
    }
  },
  methods: {
    makeActive: function(item) {
      this.active = item;
      this.$store.dispatch('makeActive', { active: this.active });
    },
    makeActivePlayer: function(item) {
      this.activePlayer = item;
    },
    makeModalActive: function() {
      this.modalActive = !this.modalActive
      this.$emit('input', this.modalActive)
    }
  },
  computed: {
  }
}
</script>

<style>
</style>
