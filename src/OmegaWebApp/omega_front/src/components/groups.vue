<template>
  <div class="col-12 groupsGlobal">
    <div class="groupContainer">
      <div class="group" v-for="group in groups">
        <div id="groupCover">
          <img v-bind:src="group.cover.source" v-if="group.cover !== undefined">
        </div>
        <div class="groupInfo">
          <span id="groupName">{{group.name}}</span>
          <div class="groupDateTime">
          </div>
          <div class="remainingTime">
          </div>
          <div class="selectGroup" v-if="group.id !== currentGroup.id" @click="setCurrentGroup(group), getFacebookPlaylists(group.id)">
            SÉLECTIONNER
          </div>
          <div class="selectGroup selected" v-if="group.id == currentGroup.id" @click="setCurrentGroup(group), getFacebookPlaylists(group.id)">
            SÉLECTIONNER
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style>
.groupsGlobal {
  height: 72vh;
  background: #0e1014;
  color: white;
  display: inline-block;
  white-space: nowrap;
  padding: 20px;
  text-align: center;
}

.group {
  width: 400px;
  background: #fff;
  display: inline-block;
  margin: 10px;
  height: 300px;
  text-overflow: ellipsis;
  text-align: left;
}

#groupCover {
  height: 100%;
  width: 150px;  
  overflow: hidden;
  float: left;
}

.groupInfo {
  float: left;
  padding-top: 30px;
  padding-left: 30px;
  color: black;
  width: 250px;
  position: relative;
  height: 100%;
}

#groupName {
  font-family: 'Montserrat-Regular';
  text-transform: uppercase;
  font-size: 24px; 
  text-overflow: ellipsis; 
  display: block; 
  overflow: hidden; 
  overflow-wrap: break-word;
  white-space: normal;
  max-height: 60px;
  width: 160px;
}

#groupLocation {
  font-family: 'Montserrat-Regular';
  text-overflow: ellipsis; 
  display: block; 
  overflow: hidden; 
  overflow-wrap: break-word;
  white-space: normal;
  font-size: 12px;
  color: #FCB42A;
  width: 150px;
}

.groupDateTime {
  position: absolute;
  top: 27px;
  right: 20px;
  text-align: center;
}

#groupDay {
  font-family: 'Montserrat-Regular';
  font-size: 30px;
  font-weight: bold;
  color: #FCB42A;
}

#groupMonth {
  font-size: 14px;
  font-family: 'Montserrat-Ultra-Light';
  text-transform: uppercase;
}

.selectGroup {
  position: absolute;
  width: 100%;
  height: 40px;
  margin: inherit;
  background: #CD5C50;
  bottom: 0;
  left: 0;
  text-align: center;
  font-family: 'Montserrat-Regular';
  font-size: 22px;
  color: #fff;
  padding-top: 5px;
  cursor: pointer;
  transition: all 0.5s ease;
}

.selectGroup:hover {
  background: #5CB85C;
}

.selected {
  transition: all 0.5s ease;
  background: #5CB85C;
}

.remainingTime {
  margin-top: 60px;
  color: silver;
  position: absolute;
  bottom: 100px;
}

#tempsRestant {
  font-family: 'Montserrat-Ultra-Light';
  font-size: 12px;
}

#daysLeft {
  font-family: 'Montserrat-Ultra-Light';
  font-size: 26px;
  margin-left: 20px;
  color: black;
}

</style>

<script>
import { mapGetters, mapActions } from 'vuex'
import FacebookApiService from '../services/FacebookApiService'
import PlaylistApiService from '../services/PlaylistApiService'
import AuthService from '../services/AuthService'


export default {
  data () {
    return {
      localGroups: '',
      facebookPlaylists: '',
      groupId: ''
    }
  },
  methods: {
    ...mapActions(['requestAsync', 'setCurrentGroup', 'sendPlaylists', 'sendGroups']),
    loadGroups: async function() {
      this.localGroups = await this.requestAsync(() => FacebookApiService.getFacebookGroups());
      this.sendGroups(this.localGroups);
    },
    getFacebookPlaylists: async function(id) {
      this.facebookPlaylists = await PlaylistApiService.getPlaylistsWithFacebook(id);
      this.sendPlaylists(this.facebookPlaylists);
    },
    getRemainingTime: function(month, day) {
      var today = new Date();
      var dd = today.getDate();
      var yy = today.getYear();
      var mm = today.getMonth();
      var hh = today.getHours();
      var m = today.getMinutes();
      var d = new Date(mm, dd, yy);
      console.log(today);

      if (today.getMonth()==11 && today.getDate()>25)
          date.setFullYear(date.getFullYear()+1)
      
      var one_day=1000*60*60*24

      for(var i = 0; i < this.localGroups.length; i++) {
        var date = new Date(today.getFullYear(), this.localGroups[i].MonthNum, this.localGroups[i].Day)
        this.localGroups[i].timeRemaining = Math.ceil((date.getTime()-today.getTime())/(one_day))
      }



    }
  },
  computed: {
    ...mapGetters(['currentGroup', 'groups'])
  },  
  created () {
    this.loadGroups()
  }
}

</script>