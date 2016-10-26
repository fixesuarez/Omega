import 'babel-polyfill'
import Vue from 'vue'
import store from './vuex/store'
import Login from './components/Login.vue'

new Vue({
  el: '#app',
  store,
  render: h => h(Login)
})
