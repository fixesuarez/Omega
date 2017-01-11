<template>
    <div>
     
        <span v-for="track in finalMix" @click="selectTrack(track)" >
           <img v-if="track.deezerId != null" v-bind:src="track.cover" height="50" width="50">
        </span>
         <div id="dz-root"></div>  
        <br>
        
          <span><router-link to="/playlist">Passer la connection</router-link><span>
<div id="dz-root"></div>
<span id='test'><router-link to="/playlist">playlist</span>
<br><br><br>
	<input type="button" @click="setDeezerPlayer()" value="Play Daft Punk - Discovery"/>
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
          width : 1520,
          height : 90,
          playlist: false,
          shuffle: true,
          onload :function(){}
        }
      });
          
  },
  methods: {
    ...mapActions(['setCurrentTrack', 'selectTrack']),
    setDPlayer: function() {
    var player = 'https://www.deezer.com/plugins/player?format=classic&autoplay=true&playlist=false&width=350&height=350&color=007FEB&layout=dark&size=small&type=tracks&id='+ this.currentTrack.deezerId +'&app_id=176241';
      console.log(this.currentTrack);
      this.DzPlayer = player;
    },
    setDeezerPlayer: function() {
      DZ.player.playTracks(this.finalPlaylist)
    }
 


  },
  created () {
    
  }
  
}
</script>