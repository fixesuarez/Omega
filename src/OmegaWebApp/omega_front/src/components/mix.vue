<template>
  <div class="col-12 mixGlobal"> 
    <div class="trackContainer">
      <div v-for="track in finalMix" @click="selectTrack(track)" >
        <img v-if="track.deezerId !== null" v-bind:src="track.cover" id="imageTrack">
      </div>
      <div id="dz-root">
        &nbsp
      </div>
      <button id="dzmix" @click="setDeezerPlayer(),PlayRandom()">Set</button>
    </div>
  </div>
  

    
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import $ from 'jquery'	
 
export default {
  data () {
    return {
      DzPlayer: ['136336110','65938270','3129782']
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
            });
        }
      }
      }); 
      DZ.Event.subscribe('current_track', function(track, evt_name){
	console.log("Currently playing track", track);
});
  },
  methods: {
    ...mapActions(['setCurrentTrack', 'selectTrack','mixToMix','sendMix','mix','addNextTrack']),
    setDeezerPlayer: function() {
      DZ.player.playTracks(this.finalPlaylist);
    },
    AddNextSong: function() {
      this.addNextTrack();
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
}

#imageTrack {
  height: 50px;
  width: 50px;
  float: left;
}

  width: 80px;
  height: 80px;
  position: relative;
  
}
#dz-root {
  position: absolute;
  bottom: 110px;
#nextTrack {
  color:white;
  background-color: black;
  height: 20px;
  width: 20px;
  position: absolute;
  z-index: 1;
}
</style>