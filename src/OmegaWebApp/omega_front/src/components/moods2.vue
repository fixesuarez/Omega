<template>
  <div class="col-12 moodsGlobal">
    <div class="col-12 moodContainer">
      <div class="currentMood" v-for="mood in moods">
        <div class="topCurrentMood">
          <img v-bind:src="mood.cover">
          <span id="currentMoodName">{{mood.rowKey}}</span>
          <span id="deleteMood">X</span>
        </div>
        <div class="middleCurrentMood">
          <span v-for="data in mood.metadonnees">
            <img v-if="data < 1 && data > 0" src="../assets/bar.png" id="dataBar" v-bind:style="{height: 150*data +'px'}">
            <img v-if="data <= 100 && data > 1" src="../assets/bar.png" id="dataBar" v-bind:style="{height: data*1.5 +'px'}">
            <img v-if="data == 1 || data == 0 && data != ''" src="../assets/bar.png" id="dataBar" v-bind:style="{height: (data*75)+75 +'px'}">
            <img v-if="data < 0" src="../assets/bar.png" id="dataBar" v-bind:style="{height: (data*150)/(-60) +'px'}">
            <img v-if="data == '' || data == null" src="../assets/bar.png" id="dataBar" v-bind:style="{height: '150px', filter: 'grayscale(100%)', opacity: '0.99'}">
          </span><br>
          <span id="dataLetter">D E S A I L P</span>
        </div>
        <div class="bottomCurrentMood" v-if="currentMood.rowKey !== mood.rowKey">
          <div class="selectMood" @click="setCurrentMood(mood)">
            SELECT
          </div>
        </div>
        <div class="bottomCurrentMood green" v-if="currentMood.rowKey == mood.rowKey">
          <div class="selectMood" @click="setCurrentMood(mood)">
            SELECT
          </div>
        </div>
      </div>
      <div class="currentMood addMood" @click="showMoodsModal(true)">
        <div class="topCurrentMood">
        </div>
        <div class="middleCurrentMood addMood" @click="showMoodsModal(true)">
          <img src="../assets/plus.png" id="plusMood">
        </div>
        <div class="bottomCurrentMood addMood" @click="showMoodsModal(true)">
          &nbsp
        </div>
      </div>
    </div>
   
    <moodModal v-if="moodsModalActive == true"><moodmodal>
  </div>
</template>


<style>
.moodsGlobal {
  height: 72vh;
  background: #0e1014;
  color: white;
}

.moodContainer {
  padding-left: 40px;
  height: 500px;
  white-space: nowrap;
  display: inline-block;
  text-align: center;
  overflow-x: auto;
  float: left;
}

.currentMood {
  display: inline-block;
  height: 380px;
  width: 220px;
  margin-right: 40px;
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
  left: 0;
  top: 0;
  position: absolute;
  color: white;
  font-family: 'Montserrat-Ultra-Light';
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
  height: 220px;
  background: white;
}

.bottomCurrentMood {
  height: 40px;
  background: #D9534F;
  letter-spacing: 4px;
  color: #fff;
  font-family: 'Montserrat-Ultra-Light';
  padding: 10px;
  cursor: pointer;
  transition: all .5s ease;
}

.bottomCurrentMood:hover {
  background: #5CB85C;
}

#dataBar {
  margin: 6px;
  width: 6px;
  margin-top: 30px;
}

.addMood {
  background: silver;
  font-family: 'Montserrat-Ultra-Light';
  color: #0e1014;
  cursor: pointer;
}

#plusMood {
  height: 120px;
}

#dataLetter {
  letter-spacing: 4px;
  color: #0e1014;
  font-family: 'Montserrat-Ultra-Light';
}

.green {
  transition: all .8s ease;
  background: #5CB85C;
}

</style>

<script>

import { mapGetters, mapActions } from 'vuex'
import MoodService from '../services/MoodService'
import moodModal from '../components/addMoodModal.vue'


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
      var result = MoodService.createMood(this.moodToCreate);
    }
  },
  computed: {
    ...mapGetters(['moods', 'currentMood', 'moodsModalActive'])
  },
  created () {
    this.loadMoods()
  },
  components: {
    moodModal
  }
}
</script>
