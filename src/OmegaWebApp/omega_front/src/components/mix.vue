<template>
  <div class="col-12 mixGlobal"> 
    <div class="trackContainer">
      <div v-for="track in finalMix" @click="selectTrack(track), addNextTrack()" class="singleTrack">
        <img v-if="track.deezerId !== null" v-bind:src="track.cover" id="imageTrack">
        <p>{{track.title}}<br><span id="albumName">{{track.albumName}}</span></p>
      </div>
         <div class="addMix">
      <span id="plusMood" @click="showMixModal(true)" v-if="finalMix.length != 0">SAVE</span>     
    </div>
    <div v-for="mix in allMix">
    <img v-bind:src="mix.parsedPlaylist[0].cover" id="imageTrack" @click="playOldMix(mix)">
      <span id="deleteMix" @click="deleteMix(mix.rowKey)"></span>
        <p>{{mix.rowKey}}<br></p>
      </div>           
    <mixModal v-if="mixModalActive == true"><mixModal>
    </div>
    <div id="dz-root">
    </div>
  </div>
  

    
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import mixModal from '../components/saveMixModal.vue'
import MixService from '../services/MixService'
import AuthService from '../services/AuthService'


import $ from 'jquery'	
 
export default {
  data () {
    return {
      DzPlayer: ['136336110','65938270','3129782'],
      mix: [],
      data: '',
    }
  },
  computed: {
    ...mapGetters(['mixModalActive', 'finalMix', 'identity', 'currentTrack','finalPlaylist','loading','allMix'])
  },
  mounted () {
    DZ.init({
      appId  : '176241',
      channelUrl : 'http://localhost:5000/mix',
      player: {
        container: 'dz-root',
        height:90,
        width:1350,
        playlist: false,
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
    ...mapActions(['setCurrentTrack','deleteMix','playOldMix', 'selectTrack','retrieveMix','mixToMix','sendMix','mix','addNextTrack','showMixModal','requestAsync']),
    setDeezerPlayer: function() {
      DZ.player.playTracks(this.finalPlaylist);
    },
    loadMix: async function() {
      var data = await this.requestAsync(() => MixService.getMix());
      this.retrieveMix(data);
    },
    deleteMix: async function(mix) {
      var data = MixService.deleteMix(mix);
      this.loadMix();
    }
  },
  created () {  
    this.loadMix()
  },
  components: {
     mixModal
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

#deleteMix {
  padding-left: 10px;
  top: 5px;
  padding: 5px;
  cursor: pointer;
  right: 5px;
  color: white;
  width: 16px;
  height: 20px;
  background-image: url("../assets/closedTrash.png");
  background-size: 16px 20px;
  transition: all .3s ease;
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
  width: 100%;
  height: 10vh;
  background-color: red;
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