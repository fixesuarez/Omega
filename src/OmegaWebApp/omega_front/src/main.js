import 'babel-polyfill'
import Vue from 'vue'
import Router from 'vue-router'
import store from './vuex/store'
import App from './App.vue'
import controlPanel from './components/controlPanel.vue'
import Login from './components/Login.vue'
import playlists from './components/playlists.vue'
import events from './components/events.vue'
import groups from './components/groups.vue'

Vue.use(Router)
Vue.use(require('vue-resource'))

const router = new Router({
  mode: 'history',
  routes: [
    { path: '', component: playlists },
    { path: '/playlists', component: playlists },
    { path: '/groups', component: groups },
    { path: '/events', component: events }
  ]
})

new Vue({
  el: '#app',
  store,
  router,
  render: h => h(App)
})
