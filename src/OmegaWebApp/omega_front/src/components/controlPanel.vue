<template>
  <div id="app">
    <html>
      <body>
        <modal v-if="modalActive == true">
          <div slot="body" class="moodsBody">
            <div class="moods">
              <div class="criterias">
                <p>creez votre propre ambiance</p>
                <p>nom de votre ambiance :<input type="text" v-model="label"></p> {{label}}
                <span class="criteriasWrapper" v-for="criteria in criterias">
                  <img v-bind:src="criteria.image"><input type="range" v-model="criteria.value"><span class="criteriaValue">{{criteria.value}}<br></span>
                </span>
                <br><br><button @click="addLocalMood({criterias, label, image, check})">Créer</button>
              </div>
            </div>
          </div>
        </modal>
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
              <span @click="sendCriterias(criterias)"><span><span @click="showModal(true)"><router-link to="" class="ambiancesTab" id="redText">ambiances</router-link></span></span></span>
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
          <div class="col-12">
            <div class="controlMoods">
              <span v-for="mood in localMoods">
                <div class="controlMood">
                    <img v-if="mood == currentMood" v-bind:src="mood.image" class="checkedImage">
                    <img v-else="mood !== currentMood" v-bind:src="mood.image" class="moodImage">
                    <div class="controlMoodOverlay" @click="setCurrentMood(mood)">
                      <span class="controlMoodLabel">{{mood.label}}</span>
                    </div>
                </div>
              </span>
              <span @click="showModal(true)"><img src="http://image.noelshack.com/fichiers/2016/46/1479417772-pluslogo.png">   
            </div>
          </div>
        </div>
        <div class="redLine">
        </div>
      </body>
    </html>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import Modal from '../components/modal.vue'

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
        { label: 'energy', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'popularity', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'instrumentalness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'speechiness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'acousticness', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'danceability', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'tempo', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'valence', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" },
        { label: 'duration_ms', value: null, image: "http://image.noelshack.com/fichiers/2016/46/1479571997-sans-titre-2.png" }
      ],
      localMoods: [
        { label: 'Lounge', image: 'https://i.ytimg.com/vi/2gwmTOdga24/hqdefault.jpg', check: false, criterias: [
          { label: 'energy', value: null },
          { label: 'popularity', value: null },
          { label: 'instrumentalness', value: null },
          { label: 'speechiness', value: null },
          { label: 'acousticness', value: null },
          { label: 'danceability', value: null },
          { label: 'tempo', value: null },
          { label: 'valence', value: null },
          { label: 'duration_ms', value: null }] },
        { label: 'Energy', image: 'http://infinite-france.com/wp-content/uploads/2015/04/I-want-Energy-concert-2015-3.jpg', check: false, criterias: [
          { label: 'energy', value: null },
          { label: 'popularity', value: null },
          { label: 'instrumentalness', value: null },
          { label: 'speechiness', value: null },
          { label: 'acousticness', value: null },
          { label: 'danceability', value: null },
          { label: 'tempo', value: null },
          { label: 'valence', value: null },
          { label: 'duration_ms', value: null }] },
        { label: 'Dance', image: 'http://tubur.com/wp-content/uploads/2016/05/girl-in-dance-mood-3d-graphic.jpg', check: false, criterias: [
          { label: 'energy', value: null },
          { label: 'popularity', value: null },
          { label: 'instrumentalness', value: null },
          { label: 'speechiness', value: null },
          { label: 'acousticness', value: null },
          { label: 'danceability', value: null },
          { label: 'tempo', value: null },
          { label: 'valence', value: null },
          { label: 'duration_ms', value: null }] },
        { label: 'Mad', image: 'https://i.ytimg.com/vi/doVoWENl7Yg/maxresdefault.jpg', check: false, criterias: [
          { label: 'energy', value: null },
          { label: 'popularity', value: null },
          { label: 'instrumentalness', value: null },
          { label: 'speechiness', value: null },
          { label: 'acousticness', value: null },
          { label: 'danceability', value: null },
          { label: 'tempo', value: null },
          { label: 'valence', value: null },
          { label: 'duration_ms', value: null }] }
      ],
      check: false,
      label: '',
      image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png'
    }
  },
  methods: {
    makeActivePlayer: function(item) {
      this.activePlayer = item;
    },
    addLocalMood: function(item) {
      this.localMoods.push(item)
    },
    ...mapActions(['makeActive', 'showModal', 'sendMoods', 'sendCriterias', 'setCurrentMood'])
  },
  computed: {
    ...mapGetters(['active', 'enabledCriterias', 'moods', 'modalActive', 'currentMood'])
  },
  created () {
  },
  components: {
    Modal
  }
}
</script>

<style src="../styles/controlPanel.css">
</style>
