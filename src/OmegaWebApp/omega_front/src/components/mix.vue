<template>
  <div> 
       
    <span v-for="track in finalMix" @click="selectTrack(track)" >
      <img v-if="track.deezerId != null" v-bind:src="track.cover" height="50" width="50">
    </span>
  
<div id="dz-root"></div>
  <button id="dzmix" @click="setDeezerPlayer(),PlayRandom()"><img id="imgmix" src="../assets/triangleGrey.png"></button>

    
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
          width : 350,
          height : 350,
          playlist: false,
          shuffle: true,
          onload :
          function DzPlay (){
           
            DZ.player.playTracks([139438215]);            
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
#dz-root {
  margin-left: 10%;
}
</style>