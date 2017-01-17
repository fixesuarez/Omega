<template>
  <div> 
       
    <span v-for="track in finalMix" @click="selectTrack(track),AddNextSong(track)" >
      <img id="CoverMix" v-if="track.deezerId != null" v-bind:src="track.cover">
    </span>

<div id="dz-root"></div>
  <button id="dzmix" @click="setDeezerPlayer()"><img id="imgmix" src="../assets/triangleGrey.png"></button>
  <button  @click="test()">test</button>
   
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
          width : 800,
          height : 90,
          //playlist: false,
          //shuffle: true,
          onload: function() {
            DZ.Event.subscribe('track_end', function(evt_name){
              console.log("track is end",DZ.player.getCurrentIndex());

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
#dzmix {
  width: 20vh;
  height: 20vh;
  margin-left:166vh;
  position: relative;
}

#imgmix {
  width: 62px;
  height: 62px;
  background-color: grey;
  
}
#CoverMix {
  width: 80px;
  height: 80px;
  position: relative;
  
}
#dz-root {
  margin-left: 10%;
}
#nextTrack {
  color:white;
  background-color: black;
  height: 20px;
  width: 20px;
  position: absolute;
  z-index: 1;
}
</style>