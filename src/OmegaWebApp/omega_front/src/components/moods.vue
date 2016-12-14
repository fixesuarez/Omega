<template>
  <transition name="modal">
    <div class="moodModal-mask">
      <div class="moodModal-wrapper">
        <div class="moodModal-container">
          <div class="moodModalText">
            <button @click="sendMoods(localMoods)">Moods</button>
            <span id="moodsHeader">Ambiances</span>
          </div>
          <div class="currentMood">
            <div class="topCurrentMood">
              <img v-bind:src="currentMood.image">
              <span id="currentMoodName">{{currentMood.label}}</span>
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
          </div>
          <div class="moods">
            <span v-for="mood in moods">
              <img v-bind:src="mood.image" @click="setCurrentMood(mood)">
            </span>
          </div>
          <button @click="showMoodsModal(false)">FERMER</button>
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
      localMoods: [
      { label: 'Lounge', image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png', 'metadonnees': {'Accousticness': '0.11', 'Danceability': '0.22', 'Energy': '0.84', 'Instrumentalness': '0.44', 'Liveness': '0.11', 'Loudness': '', 'Mode': '1', 'Popularity': ''} },
      { label: 'Energy', image: 'http://image.noelshack.com/fichiers/2016/24/1465931485-moodchill.png','metadonnees': {'Accousticness': '0.48', 'Danceability': '0.72', 'Energy': '0.84', 'Instrumentalness': '0.84', 'Liveness': '0.41', 'Loudness': '-44', 'Mode': '0', 'Popularity': '78'} },
      { label: 'Dance', image: 'http://image.noelshack.com/fichiers/2016/24/1465931498-moodsport.png', 'metadonnees': {'Accousticness': '0.95', 'Danceability': '0.52', 'Energy': '0.84', 'Instrumentalness': '0.24', 'Liveness': '0.91', 'Loudness': '-44', 'Mode': '1', 'Popularity': '18'} },
      { label: 'Mad', image: 'http://image.noelshack.com/fichiers/2016/24/1465931510-moodwork.png', 'metadonnees': {'Accousticness': '0.25', 'Danceability': '0.32', 'Energy': '0.84', 'Instrumentalness': '0.04', 'Liveness': '0.61', 'Loudness': '-44', 'Mode': '1', 'Popularity': '02'} }],
      data: ''
    }
  },
  methods: {
    ...mapActions(['showMoodsModal', 'sendMoods','setCurrentMood', 'requestAsync']),

    loadMoods: async function() {
      this.data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(this.data);
    },
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
  width: 50%;
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
}

#dataBar {
  margin: 6px;
  width: 6px;
  margin-top: 30px;
}

.moods {
  float: left;
}

.moods img {
  width: 50px;
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

</style>

