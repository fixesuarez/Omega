<template>
  <transition name="modal">
    <div class="modal-mask" @click="showEventModal(false)">
      <div class="eventModal-wrapper">
        <div class="eventModal-container">
          <div class="event" v-for="event in events">
            ee
          </div>
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
      events: [
        {'name': 'Soirée chill', 'description': 'Soirée chill chez Mamadou à base de gros saucisson des familles'},
        {'name': 'Soirée chill', 'description': 'Soirée chill chez Mamadou à base de gros saucisson des familles'},
        {'name': 'Soirée chill', 'description': 'Soirée chill chez Mamadou à base de gros saucisson des familles'}
      ]
    }
  },
  methods: {
    ...mapActions(['showEventModal'])
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

.modal-mask {
  position: fixed;
  z-index: 9999;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .5);
  transition: opacity .3s ease;
}

.eventModal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.eventModal-container {
  height: 500px;
  width: 600px;
  overflow: auto;
  margin: 0px auto;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'Montserrat-ultra-light', Arial, sans-serif;
  position: relative;
  color: black;
}

.event {
  background: red;
  width: 320px;
  height: 180px;
  margin-top: 10px;
  float: left;
}

.eventModalClose {
  position: absolute;
  right: 0;
  width: 100%;
  bottom: 0;
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