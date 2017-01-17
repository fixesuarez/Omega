<template>
  <div class="col-12 mixGlobal"> 
    <div class="trackContainer">
      <div v-for="track in finalMix" @click="selectTrack(track)" class="singleTrack">
        <img v-if="track.deezerId !== null" v-bind:src="track.cover" id="imageTrack">
        <p>{{track.title}}<br><span id="albumName">{{track.albumName}}</span></p>
      </div>
      
    </div>
    <button id="dzmix" @click="setDeezerPlayer(),PlayRandom()">Set</button>
    <div id="dz-root">
      &nbsp
    </div>
  </div>
  
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import $ from 'jquery'

export default {
  data () {
    return {
      DzPlayer: ''
    }
  },
  computed: {
    ...mapGetters(['finalMix', 'identity', 'currentTrack','finalPlaylist'])
  },
  mounted () {
    DZ.init({
      appId  : '176241',
      channelUrl : 'http://localhost:5000/mix',
      player: {
        container: 'dz-root',
        width : 1915,
        height : 90,
        playlist: false,
        shuffle: true,
        onload :
        function DzPlay () {
          DZ.player.playTracks([]);            
          DZ.player.play;
          DZ.player.setShuffle(true);
        }
      }
    });
  },
  methods: {
    ...mapActions(['setCurrentTrack', 'selectTrack','mixToMix','sendMix','mix']),
    setDPlayer: function() {
    var player = 'https://www.deezer.com/plugins/player?format=classic&autoplay=true&playlist=false&width=350&height=350&color=007FEB&layout=dark&size=small&type=tracks&id='+ this.currentTrack.deezerId +'&app_id=176241';
      console.log(this.currentTrack);
      this.DzPlayer = player;
    },
    setDeezerPlayer: function() {
      DZ.player.playTracks(this.finalPlaylist)
    },
    PlayRandom: function() {
      DZ.player.setShuffle(true)
    }
  },
  created () {
  }
}
</script>

<style>
.mixGlobal {
  height: 72vh;
  background: #0e1014;
  color: white;
  position: relative;
  font-family: 'Montserrat-Ultra-Light';
  font-size: 10px;
}

.trackContainer {
  height: 60vh;
  overflow-y: auto;
  padding: 100px;
  padding-top: 0px;
}

.singleTrack {
  height: 140px;
  width: 120px;
  float: left;
  margin: 14px;
  margin-bottom: 25px;
}

.singleTrack p {
  margin-top: 5px;
  width: 100%;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

#imageTrack {
  height: 120px;
  width: 120px;
  float: left;
  margin-bottom: 15px;
}

#albumName {
  color: silver;
}

#dz-root {
  position: absolute;
  bottom: 110px;
}
</style>