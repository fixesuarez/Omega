<template>
  <transition name="memberModal">
    <div class="memberModal-mask">
      <div class="memberModal-wrapper">
        <div class="memberModal-container">
          <div class="addMemberText">
            <div class="addMemberModal">
              <span id="memberTitle">AJOUTER UN MEMBRE</span><br>              
              <br>
              <span id="smallText">Pseudo : <input type="text" style="color: white;" v-model="member"><br><span>
              <br><br>
             <!-- <span id="membertext" class="redText">Attention, vous ne pourrez plus le modifier</span><br>      -->        
            </div>
          </div>
          <div v-if="member != ''" class="modalClose" @click="showMemberModal(false),saveMember({member}),loadMember()">
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
import FacebookApiService from '../services/FacebookApiService'

export default {
  data () {
    return {
      end: false,
      member:'',
      memberToAdd:''   
    }
  },
  methods: {
    ...mapActions(['showMemberModal','requestAsync','insertMember','sendMember']),

  },
  computed: {
    ...mapGetters(['Member'])
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

.memberModal-mask {
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

.memberModal-wrapper {
  position: absolute;
  top: 35%;
  left: 30%;
  display: table-cell;
}

.memberModal-container {
  margin-bottom: 100px;
  margin-left: 20%;
  height: 200px;
  width: 300px;
  background-color: #191B27;
  border-radius: 2px;
  box-shadow: 0 2px 8px #171717;
  transition: all .3s ease;
  font-family: 'montserrat-ultra-light', Arial, sans-serif;
  color: white;
}

.addMemberModal {
  text-align: center;
}

.addMemberText {
  height: 100%;
  padding: 30px;
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
  text-align: left;
}

.modalClose img {
  margin-left: 6px;
  width: 25px;
}

#memberTitle {
  font-size: 16px;
}

#membertext {
  font-size: 15px;
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

.memberModal-enter {
  opacity: 0;
}
.memberModal-leave-active {
  opacity: 0;
}
.memberModal-enter .memberModal-container,
.memberModal-leave-active .memberModal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

#closeModal {
  position: absolute;
  color: white;
  font-family: 'Montserrat-Ultra-Light';
  cursor: pointer
}
#closeMemberModal {
  top: 7px;
  color: white;
  font-family: 'Montserrat-Ultra-Light';
  cursor: pointer
}
</style>