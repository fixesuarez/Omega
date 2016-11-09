<template>
  <div>
    <!--<Login v-model="active"></Login>-->
    <div v-if="active === 'playlistsTab'">
    </div>
    <div v-if="active === 'evenementsTab'">
    </div>
    <div v-if="active === 'groupesTab'">
    </div>
    <!--input v-model="modalActive"></input >{{modalActive}}-->
    <router-view></router-view>
  </div>
</template>

<script>
  import Login from './components/Login.vue'
  import playlists from './components/playlists.vue'
  import moods from './components/moods.vue'
  import IncrementButton from './components/IncrementButton.vue'
  import ChooseIncrement from './components/ChooseIncrement.vue'
  import controlPanel from './components/controlPanel.vue'

  export default {
  data () {
    return {
      active: 'playlistsTab',
      modalActive: 'false'
    }
  },
  methods: {
  },
  computed: {
    count() {
      return this.$store.getters.count
    }
  },
  name: 'app',
  components: {
    Login,
    moods,
    playlists,
    IncrementButton,
    ChooseIncrement,
    controlPanel
  }
}
</script>