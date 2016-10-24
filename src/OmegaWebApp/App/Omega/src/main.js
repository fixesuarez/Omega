import 'babel-polyfill'
import $ from 'jquery'
import 'bootstrap/dist/js/bootstrap'
import Vue from 'vue'
import store from './vuex/store'
import VueRouter from 'vue-router'

import Login from './components/Login.vue'

Vue.use(VueRouter)


const router = new VueRouter({
  routes: [
    { path: '/login', component: Login },
  ]
})
/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})