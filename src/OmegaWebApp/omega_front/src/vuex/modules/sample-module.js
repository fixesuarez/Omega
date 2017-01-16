import * as types from '../mutation-types'

const state = {
  count: 0,
  nb: 3,
  choice: 0,
  active: 'playlistsTab',
  text: '',
  finalMix: [],
  playlistHelperModalActive: false,
  eventModalActive: false,
  moodsModalActive: false,
  events: '',
  groups: '',
  moods: '',
  enabledCriterias: false,
  criterias: '',
  authenticated: false,
  currentMood: '',
  currentEvent: '',
  currentGroup: '',
  currentPlaylist: '',
  currentTrack: '',
  tempoMood: '',
  checkedPlaylists: [],
  playlists: '',
  finalPlaylist: [],
  moodToInsert: '',
  mixToMix: {AmbianceName: '', AllPlaylists: ''},
  identity: false,
  track: ''
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
  [types.SENDEVENTS](state, payload) {
    state.events = payload;
  },
  [types.SENDGROUPS](state, payload) {
    state.groups = payload;
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
  [types.SETCURRENTEVENT](state, payload) {
    state.currentEvent = payload;
  },
  [types.SETCURRENTGROUP](state, payload) {
    state.currentGroup = payload;
  },
  [types.SETCURRENTPLAYLIST](state, payload) {
    state.currentPlaylist= payload;
  },
  [types.SETCURRENTTRACK](state, payload) {
    state.currentTrack = payload;
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
  },
  [types.SELECTTRACK](state, payload) {
    state.currentTrack = payload;
  },
  [types.SENDPLAYLISTS](state, payload) {
    payload = payload.map(p => { p.provider = ''; return p })
    state.checkedPlaylists = [];
    for(var i = 0; i < payload.length; i++) {
      if(payload[i].Tracks.length == 0) {
        payload[i].provider = 'd';
        payload[i].check = false;
      } else {
        payload[i].provider = payload[i].Tracks[0].RowKey.charAt(0);
        payload[i].check = true;
      }
      if(payload[i].check == true) {
        state.checkedPlaylists.push(payload[i])
      } else {
        state.checkedPlaylists.splice(state.checkedPlaylists.indexOf(payload[i]), 1)
      }
    }
    state.playlists = payload;
    state.currentPlaylist = state.playlists[0];
    // state.playlists.push.apply(state.playlists, payload);
  },
  [types.MIX](state, payload) {
    state.mixToMix.AllPlaylists = state.checkedPlaylists;
    state.mixToMix.AmbianceName = state.currentMood.rowKey;
  },
  [types.GETIDENTITY](state, payload) {
    state.identity = payload;
  },
  [types.SENDMIX](state, payload) {
    state.finalMix = payload;
    state.currentTrack = state.finalMix[0];
    for(var i=0; i<state.finalMix.length; i++){
      state.finalPlaylist.push(Number(state.finalMix[i].deezerId));
    }
  }
}

export default {
  state,
  mutations
}