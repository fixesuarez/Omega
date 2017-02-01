<template>
  <div class="col-12 eventsGlobal">
    <div class="eventContainer" id="eventContainer">
      <img src="../assets/rightButton.png" class="rightScroll" @click="scrollRight">
      <img src="../assets/leftButton.png" class="leftScroll" @click="scrollLeft">
      <div class="loadingScreen" v-if="loading == true">
        <scale-loader class="eventLoading" :loading="loading" style="margin: 20;"></scale-loader>
      </div>
      <div class="event" v-for="event in events" id="spanEvent">
        <div id="eventCover">
          <img v-bind:src="event.Cover">
        </div>
        <div class="eventInfo">
          <div class="divMoreButton" v-if="event.Type == 'eventOmega' && event.Owner == true">
            <img src="../assets/more.png" id="moreButton">
            <div id="settingsDiv">
              <span id="addMembers" @click="showMemberModal(true), sendIdToAddMember(event.RowKey)">Ajouter des membres</span><br>
              <span id="delete" @click="deleteEvent(event.RowKey)">Supprimer</span>
            </div>
          </div>
          <span id="eventName">{{event.Name}}</span>
          <span id="eventLocation">{{event.Location}}</span>
          <div class="eventDateTime">
            <span id="eventDay">{{event.Day}}</span><br>
            <span id="eventMonth">{{event.Month}}</span>
          </div>
          <div class="remainingTime">
            <span id="tempsRestant">temps restant</span><br>
            <span id="daysLeft">{{event.timeRemaining}}</span>
            jours
          </div>
          <div class="selectEvent" v-if="event.RowKey !== currentEvent.RowKey" @click="setCurrentEvent(event), getFacebookPlaylists(event.RowKey)">
            SÉLECTIONNER
          </div>
          <div class="selectEvent selected" v-if="event.RowKey == currentEvent.RowKey" @click="setCurrentEvent(event), getFacebookPlaylists(event.RowKey)">
            SÉLECTIONNER
          </div>
        </div>
      </div>
    </div><br>
    <img src="../assets/plus.png" id="plusMood" @click="showEventModal(true)">

    <addEventModal v-if="eventModalActive == true"></addEventModal>
    <addMemberModal v-if="memberModalActive == true"></addMemberModal>
  </div>
</template>

<style>
.eventsGlobal {
  height: 76vh;
  background: #0e1014;
  color: white;
  display: inline-block;
  white-space: nowrap;
  padding: 40px;
  text-align: center;
}

.eventLoading {
  position: absolute;
  left: 47%;
  top: 45%;
}

.eventContainer {
  height: 400px;
  width: 100%;
  white-space: nowrap;
  display: inline-block;
  text-align: center;
  overflow-x: auto;
}

.event {
  width: 400px;
  background: #fff;
  display: inline-block;
  margin: 10px;
  height: 300px;
  text-overflow: ellipsis;
  text-align: left;
}

#eventCover {
  height: 100%;
  width: 150px;  
  overflow: hidden;
  float: left;
}

#eventCover img {
  height: 300px;
}

.eventInfo {
  float: left;
  padding-top: 30px;
  padding-left: 30px;
  color: black;
  width: 250px;
  position: relative;
  height: 100%;
}

#eventName {
  font-family: 'Montserrat-Regular';
  text-transform: uppercase;
  font-size: 24px; 
  text-overflow: ellipsis; 
  overflow: hidden; 
  display: block; 
  overflow-wrap: break-word;
  white-space: normal;
  max-height: 100px;
  width: 160px;
}

#eventLocation {
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

.eventDateTime {
  position: absolute;
  top: 27px;
  right: 20px;
  text-align: center;
}

#eventDay {
  font-family: 'Montserrat-Regular';
  font-size: 30px;
  font-weight: bold;
  color: #FCB42A;
}

#eventMonth {
  font-size: 14px;
  font-family: 'Montserrat-Ultra-Light';
  text-transform: uppercase;
}

#moreButton {
  position: absolute;
  right: 5px;
  top: 10px;
  width: 20px;
  height: 5px;
}

#settingsDiv {
  visibility: hidden;
  background: #0e1014;
  top:10px;
  right: 10px;
  position: absolute;
  font-family: 'Montserrat-ultra-light';
  font-size: 12px;
  color: white;
  z-index: 5;
  padding-top: 10px;
  padding-bottom: 10px;
  width: 150px;
}

#settingsDiv span {
  width: 150px;
  padding-left: 10px;
  padding-right: 10px;
  cursor: pointer;
}

.divMoreButton:hover > #settingsDiv {
  visibility: visible;
}

#addMembers:hover, #delete:hover {
  color: #de002b;
}

.selectEvent {
  position: absolute;
  width: 100%;
  height: 40px;
  margin: inherit;
  background: #D9534F;
  bottom: 0;
  left: 0;
  text-align: center;
  font-family: 'Montserrat-Regular';
  font-size: 16px;
  letter-spacing: 1px;
  color: #fff;
  padding-top: 10px;
  cursor: pointer;
  transition: all 0.5s ease;
}

.selectEvent:hover {
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

#plusMood {

}

.loadingScreen {
  position: absolute;
  height: 100vh;
  width: 100%;
  top: 0;
  left: 0;
  background: black;
  opacity: 0.6;
}

</style>

<script>
import { mapGetters, mapActions } from 'vuex'
import FacebookApiService from '../services/FacebookApiService'
import PlaylistApiService from '../services/PlaylistApiService'
import AuthService from '../services/AuthService'
import addEventModal from '../components/addEventModal.vue'
import addMemberModal from '../components/addMemberModal.vue'
import ScaleLoader from 'vue-spinner/src/ScaleLoader.vue'


export default {
  data () {
    return {
      localEvents: '',
      events: [
        {'name': 'Soirée chill', 'description': 'Soirée chill chez Mamadou à base de gros saucisson des familles'},
        {'name': 'Soirée chill', 'description': 'Soirée chill chez Mamadou à base de gros saucisson des familles'},
        {'name': 'Soirée chill', 'description': 'Soirée chill chez Mamadou à base de gros saucisson des familles'}
      ],
      facebookPlaylists: '',
      eventId: ''
    }
  },
  methods: {
    ...mapActions(['showEventModal','showMemberModal','sendIdToAddMember','setLoading', 'requestAsync', 'setCurrentEvent', 'sendPlaylists', 'sendEvents']),
    loadEvents: async function() {
      this.setLoading(true);      
      this.localEvents = await this.requestAsync(() => FacebookApiService.getFacebookEvents());
      for(var i = 0; i<this.localEvents.length; i++) {
        this.localEvents[i].Day = this.localEvents[i].StartTime.charAt(8) + this.localEvents[i].StartTime.charAt(9)
        switch (this.localEvents[i].StartTime.charAt(5) + this.localEvents[i].StartTime.charAt(6)) {
          case '01':
            this.localEvents[i].Month = 'JAN';
            this.localEvents[i].MonthNum = '00';
            break;
          case '02':
            this.localEvents[i].Month = 'FEV';
            this.localEvents[i].MonthNum = '01';
            break;
          case '03':
            this.localEvents[i].Month = 'MAR';
            this.localEvents[i].MonthNum = '02';
            break;
          case '04':
            this.localEvents[i].Month = 'AVR';
            this.localEvents[i].MonthNum = '03';
            break;
          case '05':
            this.localEvents[i].Month = 'MAI';
            this.localEvents[i].MonthNum = '04';
            break;
          case '06':
            this.localEvents[i].Month = 'JUN';
            this.localEvents[i].MonthNum = '05';
            break;
          case '07':
            this.localEvents[i].Month = 'JUI';
            this.localEvents[i].MonthNum = '06';
            break;
          case '08':
            this.localEvents[i].Month = 'AOU';
            this.localEvents[i].MonthNum = '07';
            break;
          case '09':
            this.localEvents[i].Month = 'SEP';
            this.localEvents[i].MonthNum = '08';
            break;
          case '10':
            this.localEvents[i].Month = 'OCT';
            this.localEvents[i].MonthNum = '09';
            break;
          case '11':
            this.localEvents[i].Month = 'NOV';
            this.localEvents[i].MonthNum = '10';
            break;
          case '12':
            this.localEvents[i].Month = 'DEC';
            this.localEvents[i].MonthNum = '11';
            break;
        }
      }
      this.getRemainingTime();
      this.sendEvents(this.localEvents);
    },
    loadEventsFromDB: async function(id) {
      this.eventId = id;
      await PlaylistApiService.getPlaylistsWithFacebook(this.eventId);
      // this.localEvents = await this.requestAsync(() => PlaylistApiService.getPlaylistsWithFacebook());
    },
    getFacebookPlaylists: async function(id) {
      this.facebookPlaylists = await PlaylistApiService.getPlaylistsWithFacebook(id);
      this.facebookPlaylists.map(m => { this.$set(m, 'check', true); return m});
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

      for(var i = 0; i < this.localEvents.length; i++) {
        var date = new Date(today.getFullYear(), this.localEvents[i].MonthNum, this.localEvents[i].Day)
        this.localEvents[i].timeRemaining = Math.ceil((date.getTime()-today.getTime())/(one_day))
      }
    },
    scrollRight: function() {
      var scroll = document.getElementById('spanEvent').offsetWidth;
      document.getElementById('eventContainer').scrollLeft += scroll + 300;
    },
    scrollLeft: function() {
      var scroll = document.getElementById('spanEvent').offsetWidth;
      document.getElementById('eventContainer').scrollLeft -= scroll + 300;
    },
    deleteEvent: async function(id) {
      var result = await FacebookApiService.deleteEvent(id)
    }
  },
  computed: {
    ...mapGetters(['currentEvent','idToAddMember','loading', 'events', 'eventModalActive','memberModalActive'])
  },  
  created () {
    this.loadEvents()
  },
  components: {
    addEventModal,
    addMemberModal,
    ScaleLoader

  }
}

</script>