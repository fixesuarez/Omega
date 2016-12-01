import * as types from '../mutation-types'

const state = {
  count: 0,
  nb: 3,
  choice: 0,
  active: 'playlistsTab',
  text: '',
  modalActive: false,
  moods: '',
  enabledCriterias: false,
  criterias: '',
  authenticated: false,
  currentMood: '',
  currentPlaylist: {image: 'http://image.noelshack.com/fichiers/2016/48/1480455060-omegacover.png'},
  tempoMood: '',
  checkedPlaylists: [],
  playlists: []
}

const mutations = {
  [types.INCREMENT](state) {
    state.count++;
  },
  [types.DECREMENT](state) {
    state.count--;
  },
  [types.UPDATE](state) {
    state.count += state.nb;
  },
  [types.ADD](state, payload) {
    state.count += payload.choice ;
  },
  [types.MAKEACTIVE](state, payload) {
    state.active = payload;
  },
  [types.ADDTEXT](state, payload) {
    state.text = payload.text ;
  },
  [types.SHOWMODAL](state, payload) {
    state.modalActive = payload;
  },
  [types.SENDMOODS](state, payload) {
    state.moods = payload;
    state.moods.map(m => { m.check = false; return m })
  },
  [types.SENDCRITERIAS](state, payload) {
    state.criterias = payload;
  },
  [types.ENABLECRITERIAS](state, payload) {
    state.enabledCriterias = payload;
  },
  [types.ADDMOOD](state, payload) {
    state.moods.push(payload)
  },
  [types.AUTHENTICATE](state, payload) {
    state.authenticated = payload;
  },
  [types.SETCURRENTMOOD](state, payload) {
    state.currentMood = payload;
    state.currentMood.check = !state.currentMood.check;
  },
  [types.SETCURRENTPLAYLIST](state, payload) {
    state.currentPlaylist= payload;
  },
  [types.SELECTPLAYLIST](state, playlist) {
    // playlist.check = !playlist.check;
    if(playlist.check = !playlist.check) {
      state.checkedPlaylists.push(playlist)
    } else {
      state.checkedPlaylists.splice(state.checkedPlaylists.indexOf(playlist), 1)
    }
    
    // state.currentPlaylist = payload;
    // console.log(state.checkedPlaylists.length)
    // // payload.check = !payload.check;
    // var playlist = payload;

    // if(playlist.check === false) {
    //   console.log('False de base :'+playlist.check)
    //   playlist.check = true
    //   console.log('Maintenant true : '+playlist.check)
    //   state.checkedPlaylists.push(playlist)
    // } else {
    //   playlist.check = false
    //   console.log('True de base :'+playlist.check)
    //   var index = state.playlists.indexOf(playlist);
    //   state.checkedPlaylists.splice(index, 1)
    //   console.log(state.checkedPlaylists.length)
  },
  [types.SENDPLAYLISTS](state, payload) {
    // var tempoPlaylists = payload;
    // tempoPlaylists.map(m => { Vue.set(m, 'check', false); return m});
    state.playlists.push.apply(state.playlists, payload);
  }
}

export default {
  state,
  mutations
}