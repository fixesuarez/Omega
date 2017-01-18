<template>
  <transition name="moodModal">
    <div class="moodModal-mask">
      <div class="moodModal-wrapper">
        <div class="moodModal-container">
          <div class="addMoodText">
            <div class="addMoodModal">
              <span id="moodTitle">NOUVELLE AMBIANCE</span><br>
              <br>
              <span id="smallText">Nom : <input type="text" v-model="moodName"><br><span>
              <span id="smallText">Image : <input type="text" v-model="moodCover"></span>
              <input type="file" name="image">
              {{input}}
              <br><br>
              <span class="allCriterias" v-for="data in moodCriterias">
                <input v-if="data.value !== null" type="range" v-model="data.value" id="singleCriteria" v-bind:class="{active: data.value == null}"><span v-if="data.value !== null" id="criteriaValue">{{data.value}}</span>
                <input v-if="data.value == null" type="range" v-model="data.value" id="singleCriteria"><span v-if="data.value == null" id="criteriaValue">{{data.value}}</span>
              </span><br>
              <button @click="createLocalMood({moodCriterias, moodName, moodCover})">Créer</button>
            </div>
          </div>
          <div class="modalClose" @click="showMoodsModal(false)">
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
import MoodService from '../services/MoodService'

export default {
  data () {
    return {
      input: '',
      moodCover: '',
      moodName: '',
      moodCriterias: [
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
      moodToCreate: {
        'cover': null,
        'name': null, 
        'metadonnees': null
      },
      mood: {'cover': 'http://www.firstredeemer.org/wp-content/uploads/girl-backpack-thinking-sunset-field-fence-.jpg', 'name': 'Heyyy', 'metadonnees': {'Accousticness': '0.45', 'Danceability': '0.22', 'Energy': '0.84', 'Instrumentalness': '0.44', 'Liveness': '0.11', 'Loudness': '-44', 'Mode': '1', 'Popularity': '28'}}
      
    }
  },
  methods: {
    ...mapActions(['showMoodsModal', 'sendMoods','setCurrentMood', 'requestAsync', 'insertMood']),

    loadMoods: async function() {
      var data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(data);
    },
    createLocalMood: async function(item) {
      this.moodToCreate.cover = this.moodCover;
      this.moodToCreate.name = this.moodName;
      if(this.moodCriterias[0].value == null) {
        this.metadonnees.Accousticness = this.moodCriterias[0].value;
      } else {
        this.metadonnees.Accousticness = this.moodCriterias[0].value/100;
      }

      if(this.moodCriterias[1].value == null) {
        this.metadonnees.Danceability = this.moodCriterias[1].value;
      } else {
        this.metadonnees.Danceability = this.moodCriterias[1].value/100;
      }

      if(this.moodCriterias[2].value == null) {
        this.metadonnees.Energy = this.moodCriterias[2].value;
      } else {
        this.metadonnees.Energy = this.moodCriterias[2].value/100;
      }

      if(this.moodCriterias[3].value == null) {
        this.metadonnees.Instrumentalness = this.moodCriterias[3].value;
      } else {
        this.metadonnees.Instrumentalness = this.moodCriterias[3].value/100;
      }

      if(this.moodCriterias[4].value == null) {
        this.metadonnees.Speechiness = this.moodCriterias[4].value;
      } else {
        this.metadonnees.Speechiness = this.moodCriterias[4].value/100;
      }

      if(this.moodCriterias[5].value == null) {
        this.metadonnees.Liveness = this.moodCriterias[5].value
      } else {
        this.metadonnees.Liveness = this.moodCriterias[5].value/100;
      }

      if(this.moodCriterias[6].value == null) {
        this.metadonnees.Popularity = this.moodCriterias[6].value;
      } else {
        this.metadonnees.Popularity = this.moodCriterias[6].value/100;
      }
      
      this.moodToCreate.metadonnees = this.metadonnees;
      this.insertMood(this.moodToCreate);
      var result = MoodService.createMood(this.moodToCreate);
    }
  },
  computed: {
    ...mapGetters(['moods', 'currentMood'])
  },
  created () {
    this.loadMoods()
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

.moodModal-mask {
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

.moodModal-wrapper {
  position: absolute;
  top: 35%;
  left: 30%;
  display: table-cell;
}

.moodModal-container {
  margin-bottom: 100px;
  margin-left: 20%;
  height: 350px;
  width: 300px;
  background-color: #191B27;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'montserrat-ultra-light', Arial, sans-serif;
  color: white;
}

.addMoodModal {
  text-align: center;
}

.allCriterias {
}

#criteriaValue {
  position: absolute;
  margin-left: 10px;
}

.addMoodText {
  height: 100%;
  padding: 20px;
  width: 100%;
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

#moodTitle {
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

.moodModal-enter {
  opacity: 0;
}
.moodModal-leave-active {
  opacity: 0;
}
.moodModal-enter .moodModal-container,
.moodModal-leave-active .moodModal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

</style>