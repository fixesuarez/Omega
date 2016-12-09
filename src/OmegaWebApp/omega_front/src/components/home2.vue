<template>
  <div class="global">
    <div class="firstPage">
      <div class="navbar">
        <router-link to="/board" @click="login('Facebook')">FACEBOOK</router-link>
        <a href="" id="orangeText" @click="login('Facebook')">DEEZER</a>
        <a href="" @click="login('Facebook')">SPOTIFY</a>
      </div>
      <div class="header">
        <img src="../assets/triangleGrey.png" id="logo"><br>
        <div class="title"><span class="redText">o</span>mega</div>
      </div>
    </div>

    <div class="secondPage">
      test
    </div>
  </div>
</template>

<script>
import AuthService from '../services/AuthService'

export default {
  methods: {
    login(provider) {
    AuthService.login(provider);
    },
    onAuthenticated() {
    this.$router.replace('/');
    }
  }
}

</script>

<style src="../styles/home2.css">
</style>

