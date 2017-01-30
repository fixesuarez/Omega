<!--Nom, image, lieu, startime-->
<template>
  <transition name="eventModal">
    <div class="eventModal-mask">
      <div class="eventModal-wrapper">
        <div class="eventModal-container">
          <div class="addEventText">
            <div class="addEventModal">
              <span id="eventTitle">CRÉER UN évènement OMEGA</span><br>
              <br>
              <div class="newEventInfos">
                <span id="smallText">Nom : <input type="text" v-model="eventName"><br><span>
                <input v-bind:ref="avatar" type="file" name="avatar" id="inputFile" @change="upload">
                <span id="smallText">Lieu : <input type="text" v-model="eventLocation"><br><span>
                <input type="date" v-model="eventStartTime" min="2017-01-31">
              </div>
              <div class="newEvent">
                <div class="newEventCover">
                  <img :src="eventCover">
                </div>
                <div class="newEventInfo">
                  <span id="newEventName">{{eventName}}</span>
                  <span id="newEventLocation">{{eventLocation}}</span>
                </div>
              </div>

              <button @click="createEvent()">Créer</button>
            </div>
          </div>
          <div class="modalClose" @click="showEventModal(false)">
            FERMER <img src="../assets/arrow.png">
          </div>
          <!--<button class="modal-default-button" @click="showModal(false)">ok</button>-->
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import FacebookApiService from '../services/FacebookApiService'
import $ from 'jquery'

export default {
  data () {
    return {
      formData: new FormData,
      avatar: '',
      eventName: 'Nom',
      eventCover: 'Image',
      eventLocation: 'Lieu',
      eventStartTime: null,
      eventCriterias: [
        { label: 'Accousticness', value: null},
        { label: 'Danceability', value: null},
        { label: 'Energy', value: null},
        { label: 'Instrumentalness', value: null},
        { label: 'Speechiness', value: null},
        { label: 'Liveness', value: null},
        { label: 'Popularity', value: null}
      ],
      metadonnees: {'Danceability': null, 'Energy': null, 'Speechiness': null, 'Accousticness': null, 'Instrumentalness': null, 'Liveness': null, 'Popularity': null},
      data: '',
      eventToCreate: {
        'cover': null,
        'name': null, 
        'location': null
      },
      event: {'cover': 'http://www.firstredeemer.org/wp-content/uploads/girl-backpack-thinking-sunset-field-fence-.jpg', 'name': 'Heyyy', 'metadonnees': {'Accousticness': '0.45', 'Danceability': '0.22', 'Energy': '0.84', 'Instrumentalness': '0.44', 'Liveness': '0.11', 'Loudness': '-44', 'Mode': '1', 'Popularity': '28'}}
      
    }
  },
  methods: {
    ...mapActions(['showEventModal', 'sendEvents','insertEvent','setCurrentEvent', 'requestAsync', 'insertEvent']),
    upload: function(e) {
      var fichierSelectionne = document.getElementById('inputFile').files[0];
      console.log(fichierSelectionne);
      var data = new FormData();
      this.formData.append('cover', fichierSelectionne);
      
      // data.append('name', eventName);
      // data.append('location', eventLocation);
      // data.append('starttime', eventStartTime);
      // $.ajax({
      //   url: '/api/EventGroup/CreateEvent',
      //   data: data,
      //   processData: false,
      //   contentType: false,
      //   type: 'POST',
      //   success: function(data){
      //     alert(data);
      //   }
      // });
      
    },
    loadEvents: async function() {
      var data = await this.requestAsync(() => EventService.getEvents());
      this.sendEvents(data);
    },
    createEvent: async function(event) {
      this.formData.append('name', this.eventName);
      this.formData.append('location', this.eventLocation);
      this.formData.append('starttime', this.eventStartTime);
      this.insertEvent(this.formData);
      var result = FacebookApiService.createEvent(this.formData);
    },
    createLocalEvent: async function(item) {
      this.eventToCreate.cover = this.eventCover;
      this.eventToCreate.name = this.eventName;
      if(this.eventCriterias[0].value == null) {
        this.metadonnees.Accousticness = this.eventCriterias[0].value;
      } else {
        this.metadonnees.Accousticness = this.eventCriterias[0].value/100;
      }

      if(this.eventCriterias[1].value == null) {
        this.metadonnees.Danceability = this.eventCriterias[1].value;
      } else {
        this.metadonnees.Danceability = this.eventCriterias[1].value/100;
      }

      if(this.eventCriterias[2].value == null) {
        this.metadonnees.Energy = this.eventCriterias[2].value;
      } else {
        this.metadonnees.Energy = this.eventCriterias[2].value/100;
      }

      if(this.eventCriterias[3].value == null) {
        this.metadonnees.Instrumentalness = this.eventCriterias[3].value;
      } else {
        this.metadonnees.Instrumentalness = this.eventCriterias[3].value/100;
      }

      if(this.eventCriterias[4].value == null) {
        this.metadonnees.Speechiness = this.eventCriterias[4].value;
      } else {
        this.metadonnees.Speechiness = this.eventCriterias[4].value/100;
      }

      if(this.eventCriterias[5].value == null) {
        this.metadonnees.Liveness = this.eventCriterias[5].value
      } else {
        this.metadonnees.Liveness = this.eventCriterias[5].value/100;
      }

      if(this.eventCriterias[6].value == null) {
        this.metadonnees.Popularity = this.eventCriterias[6].value;
      } else {
        this.metadonnees.Popularity = this.eventCriterias[6].value/100;
      }
      
      this.eventToCreate.metadonnees = this.metadonnees;
      this.insertEvent(this.eventToCreate);
      var result = EventService.createEvent(this.eventToCreate);
    }
  },
  computed: {
    ...mapGetters(['events', 'currentEvent'])
  },
  created () {
    this.loadEvents()
  },
}

</script>

<style>
@font-face {
    font-family: 'montserrat-ultra-light';
    src:url('../assets/montserrat-ultra-light.otf');
    font-family: 'Montserrat-Regular';
    src:url('../assets/Montserrat-Regular.otf');
}

.eventModal-mask {
  position: fixed;
  z-index: 9999;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .5);
  display: table;
  transition: opacity .3s ease;
}

.eventModal-wrapper {
  position: absolute;
  top: 35%;
  left: 30%;
  display: table-cell;
}

.eventModal-container {
  margin-bottom: 100px;
  margin-left: 20%;
  height: 450px;
  width: 400px;
  background-color: #191B27;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'montserrat-ultra-light', Arial, sans-serif;
  color: white;
}

.addEventModal {
}

.allCriterias {
}

#criteriaValue {
  position: absolute;
  margin-left: 10px;
}

.addEventText {
  height: 100%;
  padding: 20px;
  width: 100%;
}

.newEvent {
  width: 350px;
  height: 200px;
  background: red;
}

.newEventCover {
  float: left; 
  width: 100px;
  height: 100%;
}

.newEventCover img {
  height: 100%;
}

.newEventInfo {
  width: 250px;
  height: 100%;
  float: left;
  text-align: left;
}

.modalClose {
  width: 180px;
  padding: 10px;
  background: #de002b;
  color: black;
  font-family: 'Montserrat-Regular';
  cursor: pointer;
  text-transform: uppercase;
}

.modalClose img {
  margin-left: 6px;
  width: 25px;
}

#eventTitle {
  font-size: 22px;
}

#smallText {
  font-size: 14px;
}

input[type="text"] {
  border: 0;
  border-bottom: 1px solid silver;
  width: auto;
  background: #191B27;
}

.eventModal-enter {
  opacity: 0;
}
.eventModal-leave-active {
  opacity: 0;
}
.eventModal-enter .eventModal-container,
.eventModal-leave-active .eventModal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

</style>