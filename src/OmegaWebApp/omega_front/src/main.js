import 'babel-polyfill'
import Vue from 'vue'
import Router from 'vue-router'
import store from './vuex/store'
import App from './App.vue'

Vue.use(Router)
Vue.use(require('vue-resource'))

new Vue({
  el: '#app',
  store,
  render: h => h(App)
})
