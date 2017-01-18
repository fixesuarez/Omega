<template>
  <div class="col-12 mixGlobal"> 
    <div class="trackContainer">
      <div v-for="track in finalMix" @click="selectTrack(track), addNextTrack()" class="singleTrack">
        <img v-if="track.deezerId !== null" v-bind:src="track.cover" id="imageTrack">
        <p>{{track.title}}<br><span id="albumName">{{track.albumName}}</span></p>
      </div>
    </div>
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
        onload : function(){
          console.log(DZ.player.getCurrentIndex());
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
  margin: 0 auto;
  display: table;
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
  width: 100%;
  position: absolute;
  bottom: 110px;
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