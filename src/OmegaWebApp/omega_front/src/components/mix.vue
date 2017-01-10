<template>
    <div>
        <span v-for="track in finalMix" @click="selectTrack(track),setDPlayer()">
           <img v-if="track.deezerId != null" v-bind:src="track.cover" height="50" width="50">
        </span>
       <iframe scrolling="no" frameborder="0" allowTransparency="true" v-bind:src="DzPlayer" width="350" height="150"></iframe>
          <span><router-link to="/playlist">Passer la connection</router-link><span>

<span id='test'><router-link to="/playlist">playlist</span>

    </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
export default {
      data () {
    return {
      DzPlayer: ''
      }
  },
  computed: {
      ...mapGetters(['finalMix', 'identity', 'currentTrack']),
    },
    created () {
  },
  methods: {
    ...mapActions(['setCurrentTrack', 'selectTrack']),
    setDPlayer: function() {
    var player = 'https://www.deezer.com/plugins/player?format=classic&autoplay=true&playlist=true&width=350&height=350&color=007FEB&layout=dark&size=medium&type=tracks&id='+ this.currentTrack.deezerId +'&app_id=176241';
      this.DzPlayer = player;
    }
  }
}
</script>