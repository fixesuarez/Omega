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
              <span @click="sendCriterias(criterias)"><span @click="sendMoods(moods)"><span @click="showModal(true)"><router-link to="" class="ambiancesTab" id="redText">ambiances</router-link></span></span></span>
            </nav>
          </div>
          <div class="col-12">
           <!-- <div class="players">
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
          </div>-->
        </div>
        <div class="redLine">
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
      groupesTab: 'groupesTab',
      true: true,
      criterias: [
        { label: 'energy', value: null },
        { label: 'popularity', value: null },
        { label: 'instrumentalness', value: null },
        { label: 'speechiness', value: null },
        { label: 'acousticness', value: null },
        { label: 'danceability', value: null },
        { label: 'tempo', value: null },
        { label: 'valence', value: null },
        { label: 'duration_ms', value: null }
      ],
      moods: [
        { label: 'Lounge', image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png', criterias: [
          { label: 'energy', value: null },
          { label: 'popularity', value: null },
          { label: 'instrumentalness', value: null },
          { label: 'speechiness', value: null },
          { label: 'acousticness', value: null },
          { label: 'danceability', value: null },
          { label: 'tempo', value: null },
          { label: 'valence', value: null },
          { label: 'duration_ms', value: null }] },
        { label: 'Energy', image: 'http://image.noelshack.com/fichiers/2016/24/1465931485-moodchill.png', criterias: [
          { label: 'energy', value: null },
          { label: 'popularity', value: null },
          { label: 'instrumentalness', value: null },
          { label: 'speechiness', value: null },
          { label: 'acousticness', value: null },
          { label: 'danceability', value: null },
          { label: 'tempo', value: null },
          { label: 'valence', value: null },
          { label: 'duration_ms', value: null }] },
        { label: 'Dance', image: 'http://image.noelshack.com/fichiers/2016/24/1465931498-moodsport.png', criterias: [
          { label: 'energy', value: null },
          { label: 'popularity', value: null },
          { label: 'instrumentalness', value: null },
          { label: 'speechiness', value: null },
          { label: 'acousticness', value: null },
          { label: 'danceability', value: null },
          { label: 'tempo', value: null },
          { label: 'valence', value: null },
          { label: 'duration_ms', value: null }] },
        { label: 'Mad', image: 'http://image.noelshack.com/fichiers/2016/24/1465931510-moodwork.png', criterias: [
          { label: 'energy', value: null },
          { label: 'popularity', value: null },
          { label: 'instrumentalness', value: null },
          { label: 'speechiness', value: null },
          { label: 'acousticness', value: null },
          { label: 'danceability', value: null },
          { label: 'tempo', value: null },
          { label: 'valence', value: null },
          { label: 'duration_ms', value: null }] }
      ]
    }
  },
  methods: {
    makeActivePlayer: function(item) {
      this.activePlayer = item;
    },
    ...mapActions(['makeActive', 'showModal', 'sendMoods', 'sendCriterias'])
  },
  computed: {
    ...mapGetters(['active', 'enabledCriterias'])
  }
}
</script>

<style src="../styles/controlPanel.css">
</style>
