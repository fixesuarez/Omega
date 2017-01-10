import * as types from '../mutation-types'

const state = {
  count: 0,
  nb: 3,
  choice: 0,
  active: 'playlistsTab',
  text: '',
  playlistHelperModalActive: false,
  eventModalActive: false,
  moodsModalActive: false,
  moods: '',
  enabledCriterias: false,
  criterias: '',
  authenticated: false,
  currentMood: '',
  currentPlaylist: {image: 'http://image.noelshack.com/fichiers/2016/48/1480455060-omegacover.png'},
  tempoMood: '',
  checkedPlaylists: [],
  playlists: [],
  moodToInsert: '',
  mixToMix: {AmbianceName: '', AllPlaylists: ''},
  identity: false
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
  [types.SHOWPLAYLISTHELPERMODAL](state, payload) {
    state.playlistHelperModalActive = payload;
  },
  [types.SHOWEVENTMODAL](state, payload) {
    state.eventModalActive = payload;
  },
  [types.SHOWMOODSMODAL](state, payload) {
    state.moodsModalActive = payload;
  },
  [types.SENDMOODS](state, payload) {
    state.moods = payload;
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
  [types.INSERTMOOD](state, payload) {
    state.moodToInsert= payload;
  },
  [types.SELECTPLAYLIST](state, playlist) {
    state.currentPlaylist = playlist;
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
    payload = payload.map(p => { p.provider = ''; return p })
    console.log(JSON.stringify(payload))
    for(var i = 0; i < payload.length; i++) {
      if(payload[i].Tracks.length == 0) {
        payload[i].provider = 'd';
      } else {
        payload[i].provider = payload[i].Tracks[0].RowKey.charAt(0);
      }
    }
    state.playlists.push.apply(state.playlists, payload);
  },
  [types.MIX](state, payload) {
    state.mixToMix.AllPlaylists = state.checkedPlaylists;
    state.mixToMix.AmbianceName = state.currentMood.rowKey;
  },
  [types.GETIDENTITY](state, payload) {
    state.identity = payload;
  }
}

export default {
  state,
  mutations
}