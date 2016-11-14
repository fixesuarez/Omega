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
              <span @click="makeActive('playlistsTab')"><router-link to="/playlists" class="playlistsTab">playlists</router-link></span>
              <span @click="makeActive('evenementsTab')"><router-link to="/events" class="evenementsTab" >evenements</router-link></span>
              <span @click="makeActive('groupesTab')"><router-link to="/groups" class="groupesTab" >groupes</router-link></span>
              <a href="#" class="ambiancesTab" id="redText" v-on:click="makeModalActive()">ambiances</a>
            </nav>
          </div>
          <div class="col-12">
            <div class="players">
              <span v-if="activePlayer === 'deezerPlayer'">
                <iframe class="spotifyPlayer" style="opacity: 0.2" src="https://embed.spotify.com/?uri=spotify:track:2V65y3PX4DkRhy1djlxd9p" width="250" height="80" frameborder="0" allowtransparency="true"></iframe>
                <iframe class="deezerPlayer" style="opacity: 1" scrolling="no" frameborder="0" allowTransparency="true" src="https://www.deezer.com/plugins/player?format=classic&autoplay=false&playlist=false&width=250&height=200&color=007FEB&layout=dark&size=medium&type=playlist&id=30595446&app_id=1" width="250" height="62"></iframe>
              </span>
              <span v-if="activePlayer === 'spotifyPlayer'">
                <iframe class="spotifyPlayer" style="opacity: 1" src="https://embed.spotify.com/?uri=spotify:track:2V65y3PX4DkRhy1djlxd9p" width="250" height="80" frameborder="0" allowtransparency="true"></iframe>
                <iframe class="deezerPlayer" style="opacity: 0.2" scrolling="no" frameborder="0" allowTransparency="true" src="https://www.deezer.com/plugins/player?format=classic&autoplay=false&playlist=false&width=250&height=200&color=007FEB&layout=dark&size=medium&type=playlist&id=30595446&app_id=1" width="250" height="62"></iframe>
              </span>
              <span v-if="activePlayer === ''">
                <iframe class="spotifyPlayer" style="opacity: 1" src="https://embed.spotify.com/?uri=spotify:track:2V65y3PX4DkRhy1djlxd9p" width="250" height="80" frameborder="0" allowtransparency="true"></iframe>
                <iframe class="deezerPlayer" style="opacity: 1" scrolling="no" frameborder="0" allowTransparency="true" src="https://www.deezer.com/plugins/player?format=classic&autoplay=false&playlist=false&width=250&height=200&color=007FEB&layout=dark&size=medium&type=playlist&id=30595446&app_id=1" width="250" height="62"></iframe>
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
import { mapGetters, mapActions } from 'vuex'

export default {
  data () {
    return {
      activePlayer: '',
      modalActive: false,
      playlistsTab: 'playlistsTab',
      evenementsTab: 'evenementsTab',
      groupesTab: 'groupesTab'
    }
  },
  methods: {
    makeActivePlayer: function(item) {
      this.activePlayer = item;
    },
    makeModalActive: function() {
      this.modalActive = !this.modalActive
      this.$emit('input', this.modalActive)
    },
    ...mapActions(['makeActive'])
  },
  computed: {
    ...mapGetters(['active'])
  }
}
</script>

<style src="../styles/controlPanel.css">
</style>
