<template>
  <div>
    <!--<Login v-model="active"></Login>-->
    <controlPanel></controlPanel>
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
  import { mapGetters, mapActions } from 'vuex'

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
    ...mapGetters(['active'])
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