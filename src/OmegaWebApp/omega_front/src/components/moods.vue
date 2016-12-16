<template>
  <transition name="modal">
    <div class="moodModal-mask">
      <div class="moodModal-wrapper">
        <div class="moodModal-container">
          <span @click="showMoodsModal(false)" id="closeButton">X</span>
          <div class="moodModalText">
            <!--<button @click="sendMoods(moods)">Moods</button>-->
            <span id="moodsHeader">Ambiances</span>
          </div>
          <div class="currentMood">
            <div class="topCurrentMood">
              <img v-bind:src="currentMood.cover">
              <span id="currentMoodName">{{currentMood.rowKey}}</span>
              <span id="deleteMood">X</span>
            </div>
            <div class="middleCurrentMood">
              <span v-for="data in currentMood.metadonnees">
                <img v-if="data < 1 && data > 0" src="../assets/bar.png" id="dataBar" v-bind:style="{height: 150*data +'px'}">
                <img v-if="data <= 100 && data > 1" src="../assets/bar.png" id="dataBar" v-bind:style="{height: data*1.5 +'px'}">
                <img v-if="data == 1 || data == 0 && data != ''" src="../assets/bar.png" id="dataBar" v-bind:style="{height: (data*75)+75 +'px'}">
                <img v-if="data < 0" src="../assets/bar.png" id="dataBar" v-bind:style="{height: (data*150)/(-60) +'px'}">
                <img v-if="data == '' || data == null" src="../assets/bar.png" id="dataBar" v-bind:style="{height: '150px', filter: 'grayscale(100%)', opacity: '0.99'}">
              </span>
            </div>
            <div class="bottomCurrentMood">
              Bottom here
            </div>
          </div>
          <div class="moods">
            <span v-for="mood in moods">
              <!--<span @click="setCurrentMood(mood)">{{mood.rowKey}}</span>-->
              <img v-bind:src="mood.cover" @click="setCurrentMood(mood)">
            </span>
          </div><br>
          <div class="addMood">
            Créer une Ambiance<br>
            <span id="smallText">Nom : <input type="text" v-model="moodName"><br><span>
            <span id="smallText">Image : <input type="text" v-model="moodCover"></span>
            <span class="allCriterias" v-for="data in moodCriterias">
              <input type="range" v-model="data.value" id="singleCriteria" v-bind:class="{active: data.value == null}"><span v-if="data.value !== null" id="criteriaValue">{{data.value}}</span>
              <!--<input v-if="data.value == null" type="range" v-model="data.value" id="singleCriteria"><span v-if="data.value == null" id="criteriaValue">{{data.value}}</span>-->
            </span><br>
            <button @click="createLocalMood({moodCriterias, moodName, moodCover})">Créer</button>
            {{metadonnees}}
          </div>
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
      moodCover: '',
      moodName: '',
      moodCriterias: [
        { label: 'Accousticness', value: null},
        { label: 'Danceability', value: null},
        { label: 'Energy', value: null},
        { label: 'Instrumentalness', value: null},
        { label: 'Speechiness', value: null},
        { label: 'Liveness', value: null},
        { label: 'Loudness', value: null},
        { label: 'Popularity', value: null}
      ],
      metadonnees: {'Accousticness': null, 'Danceability': null, 'Energy': null, 'Instrumentalness': null, 'Speechiness': null, 'Liveness': null, 'Loudness': null, 'Popularity': null},
      localMoods: [
      { label: 'Lounge', image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png', 'metadonnees': {'Accousticness': '0.11', 'Danceability': '0.22', 'Energy': '0.84', 'Instrumentalness': '0.44', 'Liveness': '0.11', 'Loudness': '', 'Mode': '1', 'Popularity': ''} },
      { label: 'Energy', image: 'http://image.noelshack.com/fichiers/2016/24/1465931485-moodchill.png','metadonnees': {'Accousticness': '0.48', 'Danceability': '0.72', 'Energy': '0.84', 'Instrumentalness': '0.84', 'Liveness': '0.41', 'Loudness': '-44', 'Mode': '0', 'Popularity': '78'} },
      { label: 'Dance', image: 'http://image.noelshack.com/fichiers/2016/24/1465931498-moodsport.png', 'metadonnees': {'Accousticness': '0.95', 'Danceability': '0.52', 'Energy': '0.84', 'Instrumentalness': '0.24', 'Liveness': '0.91', 'Loudness': '-44', 'Mode': '1', 'Popularity': '18'} },
      { label: 'Mad', image: 'http://image.noelshack.com/fichiers/2016/24/1465931510-moodwork.png', 'metadonnees': {'Accousticness': '0.25', 'Danceability': '0.32', 'Energy': '0.84', 'Instrumentalness': '0.04', 'Liveness': '0.61', 'Loudness': '-44', 'Mode': '1', 'Popularity': '02'} }],
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
      this.data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(this.data);
    },
    createLocalMood: function(item) {
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
        this.metadonnees.Loudness = this.moodCriterias[6].value
      } else {
        this.metadonnees.Loudness = (this.moodCriterias[6].value)/(-100)*60;
      }

      if(this.moodCriterias[7].value == null) {
        this.metadonnees.Popularity = this.moodCriterias[7].value;
      } else {
        this.metadonnees.Popularity = this.moodCriterias[7].value/100;
      }
      
      this.moodToCreate.metadonnees = this.metadonnees;
      this.insertMood(this.moodToCreate);
      this.$http.post('http://localhost:5000/api/Ambiance/InsertAmbiance', this.moodToCreate, function () {
       })
    }
  },
  computed: {
    ...mapGetters(['moods', 'currentMood'])
  },
  created () {
    if(this.moods.length === 0) {
      this.loadMoods()
    }
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
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .5);
  display: table;
  transition: opacity .3s ease;
}
.moodModal-wrapper {
  display: table-cell;
  vertical-align: middle;
}
.moodModal-container {
  width: 650px;
  overflow: auto;
  margin: 0px auto;
  padding: 40px 60px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'Montserrat-ultra-light', Arial, sans-serif;
  color: black;
}

.moodModalText {
}

#moodsHeader {
  font-size: 24px;
}

.currentMood {
  box-shadow: 0px 0px 10px silver;
  height: 400px;
  width: 220px;
  margin-right: 20px;
  margin-top: 20px;
  float: left;
  overflow: hidden;
  border-radius: 3px;
}

.topCurrentMood {
  overflow: hidden;
  position: relative;
  height: 120px;
}

.topCurrentMood img {
  width: 100%;
  filter: brightness(50%);
}

#currentMoodName {
  margin-top: 95px;
  padding-left: 10px;
  top: 0;
  position: absolute;
  color: white;
}

#deleteMood {
  padding-left: 10px;
  top: 0;
  padding: 5px;
  cursor: pointer;
  right: 0;
  position: absolute;
  color: white;
}

.middleCurrentMood {
  text-align: center;
  height: 200px;
}

.bottomCurrentMood {
  height: 80px;
  background: #ffe;
}

#dataBar {
  margin: 6px;
  width: 6px;
  margin-top: 30px;
}

#closeButton {
  float: right;
}

.moods {
  margin-top: 20px;
  float: left;
  width: 250px;
  height: 180px;
  overflow: auto;
}

.moods img {
  width: 80px;
  padding: 1px;
  height: 50px;
}

.addMood {
  margin-top: 0px;
  height: 220px;
  float: left;
  width: 280px;
  font-size: 14px;
}

#smallText {
  margin-left: 16px;
  font-size: 12px;
}

.allCriterias {
  margin-left: 18px;
}

#singleCriteria {
  margin-top: 10px;
}

.active {
  filter: grayscale(100%);
  opacity: 0.3;
}

#criteriaValue {
  margin-left: 10px;
  font-size: 12px;
}

.modalClose {
  width: 180px;
  padding: 10px;
  background: #de002b;
  color: black;
  font-family: 'Montserrat-Regular';
  cursor: pointer;
}

.modalClose img {
  margin-left: 6px;
  width: 25px;
}

.modal-enter {
  opacity: 0;
}
.modal-leave-active {
  opacity: 0;
}
.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

input[type="text"] {
  border: 0;
  border-bottom: 1px solid silver;
  width: auto;
}
</style>

