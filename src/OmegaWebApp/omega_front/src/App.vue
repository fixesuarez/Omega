<template>
  <div>
    <!--<Login v-model="active"></Login>-->
    <!--<controlPanel></controlPanel>-->
    <div v-if="active === 'playlistsTab'">
    </div>
    <div v-if="active === 'evenementsTab'">
    </div>
    <div v-if="active === 'groupesTab'">
    </div>
    <!--input v-model="modalActive"></input >{{modalActive}}-->
    <router-view></router-view>
    <modal v-if="modalActive == true" @close="modalActive == false" class="moodsModal">
      <div slot="header" class="moodsHeader">ambiances</div>
      <div slot="body" class="moodsBody">
        <p>choisissez une ambiance pour votre mix</p>
        <div class="moods">
          <span v-for="mood in moods">
            <div class="mood">
              <img v-bind:src="mood.image">
              <div class="moodOverlay">
                <span class="moodLabel">{{mood.label}}</span>
              </div>
            </div>
          </span>
          <span @click="enableCriterias(true)"><img src="http://image.noelshack.com/fichiers/2016/46/1479417772-pluslogo.png"></span>
          <div class="criterias" v-if="enabledCriterias == true">
            <p>creez votre propre ambiance</p>
            <p>nom de votre ambiance :<input type="text" v-model="moodName"></p> {{moodName}}
            <span class="criteriasWrapper" v-for="criteria in localCriterias">
              <img v-bind:src="criteria.image"><input type="range" v-model="criteria.value"><span class="criteriaValue">{{criteria.value}}<br></span>
            </span>
            <br><br><button @click="addMood(localCriterias, moodName)">Créer</button>
          </div>
        </div>
        <!--<span class="static" v-for="mood in moods" v-bind:class="{active: isActive}">{{mood.label}}</span>-->
      </div>
    </modal>
  </div>
</template>

<script>
  import Login from './components/Login.vue'
  import playlists from './components/playlists.vue'
  import moods from './components/moods.vue'
  import Modal from './components/modal.vue'
  import IncrementButton from './components/IncrementButton.vue'
  import ChooseIncrement from './components/ChooseIncrement.vue'
  import controlPanel from './components/controlPanel.vue'
  import { mapGetters, mapActions } from 'vuex'

  export default {
  data () {
    return {
      active: 'playlistsTab',
      isActive: true,
      true: true,
      moodName: '',
      localCriterias: [
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
    }
  },
  methods: {
    ...mapActions(['enableCriterias', 'test', 'addMood']),
  },
  computed: {
    ...mapGetters(['active', 'modalActive', 'moods', 'test', 'enabledCriterias', 'criterias'])
  },
  name: 'app',
  components: {
    Login,
    Modal,
    playlists,
    IncrementButton,
    ChooseIncrement,
    controlPanel
  }
}
</script>

<style src="./styles/app.css">
</style>
</style>