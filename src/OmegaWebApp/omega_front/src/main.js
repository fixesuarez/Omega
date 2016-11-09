import 'babel-polyfill'
import Vue from 'vue'
import Router from 'vue-router'
import store from './vuex/store'
import App from './App.vue'
import controlPanel from './components/controlPanel.vue'
import Login from './components/Login.vue'

Vue.use(Router)
Vue.use(require('vue-resource'))

const router = new Router({
  mode: 'history',
  routes: [
    { path: '', component: controlPanel },
    { path: '/login', component: Login }
  ]
})

new Vue({
  el: '#app',
  store,
  router,
  render: h => h(App)
})
