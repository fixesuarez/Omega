<template>
  <transition name="pseudoModal">
    <div class="pseudoModal-mask">
      <div class="pseudoModal-wrapper">
        <div class="pseudoModal-container">
          <div class="addPseudoText">
            <div class="addPseudoModal">
              <span id="pseudoTitle">CHANGER VOTRE PSEUDO</span><br>
              <br>
              <span id="smallText">Pseudo : <input type="text" style="color: white;" v-model="pseudo"><br><span>
              <br><br>
            </div>
          </div>
          <div v-if="pseudo != ''" class="modalClose" @click="showPseudoModal(false),savePseudo({pseudo})">
            SAUVEGARDER <img src="../assets/arrow.png" >
          </div>
          <!--<button class="modal-default-button" @click="showModal(false)">ok</button>-->
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import PseudoService from '../services/PseudoService'

export default {
  data () {
    return {
      pseudo:'',
      pseudoToSave:''   
    }
  },
  methods: {
    ...mapActions(['showPseudoModal','requestAsync','insertPseudo']),

  /*  loadMoods: async function() {
      var data = await this.requestAsync(() => MoodService.getMoods());
      this.sendMoods(data);
    },*/
    savePseudo: async function(item) {
      this.pseudoToSave= this.pseudo;
     // this.insertPseudo(this.pseudoToSave);
      var result = PseudoService.SavePseudo(this.pseudoToSave);
    }
  },
  computed: {
    ...mapGetters(['Pseudo'])
  },
  created () {
    
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

.pseudoModal-mask {
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

.pseudoModal-wrapper {
  position: absolute;
  top: 35%;
  left: 30%;
  display: table-cell;
}

.pseudoModal-container {
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

.addPseudoModal {
  text-align: center;
}

.addPseudoText {
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

#pseudoTitle {
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

.pseudoModal-enter {
  opacity: 0;
}
.pseudoModal-leave-active {
  opacity: 0;
}
.pseudoModal-enter .pseudoModal-container,
.pseudoModal-leave-active .pseudoModal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

</style>