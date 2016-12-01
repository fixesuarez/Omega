import 'babel-polyfill'
import Vue from 'vue'
import Router from 'vue-router'
import store from './vuex/store'
import App from './App.vue'
import controlPanel from './components/controlPanel.vue'
import Login from './components/Login.vue'
import Home from './components/Home.vue'
import playlists from './components/playlists.vue'
import events from './components/events.vue'
import groups from './components/groups.vue'
import AuthService from './services/AuthService'

Vue.use(Router)
Vue.use(require('vue-resource'))

function requireAuth (to, from, next)  {
  if (!AuthService.isConnected) {
    next({
      path: '/login',
      query: { redirect: to.fullPath }
    });

    return;
  }

  var requiredProviders = to.meta.requiredProviders;

  if(requiredProviders && !AuthService.isBoundToProvider(requiredProviders)) {
    next( false )
  };

  next();
}

const router = new Router({
  mode: 'history',
  routes: [
    { path: '', component: Home },
    { path: '/playlists', component: playlists },
    { path: '/groups', component: groups },
    { path: '/events', component: events }
  ]
})

AuthService.allowedOrigins = ['http://localhost:5000'];

//AuthService.logoutEndpoint = '/Account/LogOff';

AuthService.providers = {
  //'Base': {
  //  endpoint: '/Account/Login'
  //},
  'Facebook': {
    endpoint: '/Account/ExternalLogin?provider=Facebook'
  },
  'Deezer': {
    endpoint: '/Account/ExternalLogin?provider=Deezer'
  },
  'Spotify': {
    endpoint: '/Account/ExternalLogin?provider=Spotify'
  },
};

new Vue({
  el: '#app',
  store,
  router,
  render: h => h(App)
})
