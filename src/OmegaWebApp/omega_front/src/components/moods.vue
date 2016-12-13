<template>
  <transition name="modal">
    <div class="moodModal-mask">
      <div class="moodModal-wrapper">
        <div class="moodModal-container">
          <div class="moodModalText">
            Voici vos playlists<br>Selectionnez celles que vous voulez garder pour votre mix
          </div>
          <div class="modalClose" @click="showPlaylistHelperModal(false)">
            COMMENCER <img src="../assets/arrow.png">
          </div>
          <!--<button class="modal-default-button" @click="showModal(false)">ok</button>-->
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'

export default {
  data () {
    return {
      moods: [
      { label: 'Lounge', image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png' },
      { label: 'Energy', image: 'http://image.noelshack.com/fichiers/2016/24/1465931485-moodchill.png' },
      { label: 'Dance', image: 'http://image.noelshack.com/fichiers/2016/24/1465931498-moodsport.png' },
      { label: 'Mad', image: 'http://image.noelshack.com/fichiers/2016/24/1465931510-moodwork.png' }]
    }
  },
  methods: {
    ...mapActions(['showPlaylistHelperModal'])
  }
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
  left: 12%;
  display: table-cell;
}

.moodModal-container {
  margin-bottom: 100px;
  margin-left: 20%;
  height: 40px;
  width: 180px;
  background-color: #191B27;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'montserrat-ultra-light', Arial, sans-serif;
  color: white;
}

.modalText {
  width: 300px;
  padding: 10px;
  background: #191B27;
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

