<template>
  <transition name="modal">
    <div class="modal-mask">
      <div class="modal-wrapper">
        <div class="modal-container">

          <div class="modal-header">
            <slot name="header">
              Default header
            </slot>
          </div>

          <div class="modal-body">
            <slot name="body">
              Default body
            </slot>
          </div>

          <div class="modal-footer">
            <slot name="footer">
              <span @click="enableCriterias(false)"><button class="modal-default-button" @click="showModal(false)" ref="okButton">
                OK
              </button></span>
            </slot>
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
      moods: [
      { label: 'Lounge', image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png' },
      { label: 'Energy', image: 'http://image.noelshack.com/fichiers/2016/24/1465931485-moodchill.png' },
      { label: 'Dance', image: 'http://image.noelshack.com/fichiers/2016/24/1465931498-moodsport.png' },
      { label: 'Mad', image: 'http://image.noelshack.com/fichiers/2016/24/1465931510-moodwork.png' }]
    }
  },
  methods: {
    ...mapActions(['showModal', 'enableCriterias'])
  },
  computed: {
    ...mapGetters(['modalActive', 'moods'])
  },
  mounted () {
    this.$refs.okButton.focus()
  }
}
</script>

<style scoped>

@font-face {
    font-family: 'beat';
    src:url('../assets/beat.otf');
}

.modal-mask {
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
.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}
.modal-container {
  width: 500px;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'beat', Arial, sans-serif;
  text-align: center;
}

.modal-body {
  text-align: center;
}

.modal-header h3 {
  margin-top: 0;
  color: #171717;
}
.modal-body p {
  margin: 20px 0;
}
.modal-footer {
  height: 30px;
  margin-top: 10px;
}
.modal-default-button {
  float: right;
}
/*
 * The following styles are auto-applied to elements with
 * transition="modal" when their visibility is toggled
 * by Vue.js.
 *
 * You can easily play with the modal transition by editing
 * these styles.
 */
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
Contact GitHub API Training Shop Blog About
© 2016 GitHub, Inc. Terms Privacy Security Status Help