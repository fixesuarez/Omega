import * as types from '../mutation-types'

const state = {
  loading: false,
  facebookConnected: '',
  deezerConnected: '',
  spotifyConnected: '',
  finalMix: [],
  allMix: '',
  playlistHelperModalActive: false,
  pseudoModalActive: true,
  mixModalActive: false,  
  eventModalActive: false,
  moodsModalActive: false,
  events: '',
  groups: '',
  pseudo: '',
  moods: '',
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
  nextTrack: [],
  moodToInsert: '',
  mixToInsert: '',
  mixToMix: {AmbianceName: '', AllPlaylists: ''},
  identity: false,
  track: ''
}

const mutations = {
  [types.SETCONNECTION](state, payload) {
    if(payload == 'Facebook')
      state.facebookConnected = true;
    else if(payload == 'Deezer')
      state.deezerConnected = true;
    else 
      state.spotifyConnected = true;
  },

//MODALS SHOW

  [types.SHOWPLAYLISTHELPERMODAL](state, payload) {
    state.playlistHelperModalActive = payload;
  },
  [types.SHOWEVENTMODAL](state, payload) {
    state.eventModalActive = payload;
  },
  [types.SHOWMOODSMODAL](state, payload) {
    state.moodsModalActive = payload;
  },
  [types.SHOWMIXMODAL](state, payload) {
    state.mixModalActive = payload;
  },
  [types.SHOWPSEUDOMODAL](state, payload) {
    state.pseudoModalActive = payload;
  },

//DATA SENDING

  [types.SENDMOODS](state, payload) {
    state.moods = payload;
  },
  [types.SENDMIX](state, payload) {
    state.mix = payload;
  },
  [types.SENDEVENTS](state, payload) {
    state.events = payload;
  },
  [types.SENDGROUPS](state, payload) {
    state.groups = payload;
  },
  [types.SENDPSEUDO](state, payload) {
    state.pseudo = payload;
  },
  [types.SENDCRITERIAS](state, payload) {
    state.criterias = payload;
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
  },
  [types.SENDMIX](state, payload) {
    state.finalPlaylist = [];
    state.finalMix = payload;
    state.currentTrack = state.finalMix[0];

    for(var i=0; i<state.finalMix.length; i++){
      state.finalPlaylist.push(Number(state.finalMix[i].deezerId));
    }
    DZ.player.playTracks(state.finalPlaylist);
  },

//SETTERS

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

//INSERTS

  [types.INSERTMOOD](state, payload) {
    state.moodToInsert= payload;
  },
  [types.INSERTMIX](state, payload) {
    state.mixToInsert = payload;
  },

//GETTERS 
  [types.GETIDENTITY](state, payload) {
    state.identity = payload;
  },
  [types.RETRIEVEMIX](state, payload) {
    state.allMix = payload;
  },
  [types.GETPSEUDO](state, payload) {
    state.pseudo = payload;
  },

//SELECTERS

  [types.SELECTPLAYLIST](state, playlist) {
    if(playlist.check = !playlist.check) {
      state.checkedPlaylists.push(playlist)
    } else {
      state.checkedPlaylists.splice(state.checkedPlaylists.indexOf(playlist), 1)
    }
  },
  [types.SELECTTRACK](state, payload) {
    state.currentTrack = payload.deezerId;
  },

//ACTIONS

  [types.ADDMOOD](state, payload) {
    state.moods.push(payload)
  },
  [types.AUTHENTICATE](state, payload) {
    state.authenticated = payload;
  },
  [types.MIX](state, payload) {
    state.mixToMix.AllPlaylists = state.checkedPlaylists;
    state.mixToMix.AmbianceName = state.currentMood.rowKey;
  },
  [types.PLAYOLDMIX](state, payload) {
    state.finalMix = [];
    var a = state.allMix.indexOf(payload);
   for(var i=0; i<state.allMix[a].parsedPlaylist.length; i++){
      state.finalMix.push(state.allMix[a].parsedPlaylist[i])
    }
        state.finalPlaylist = [];
       state.currentTrack = state.finalMix[0];

    for(var i=0; i<state.finalMix.length; i++){
      state.finalPlaylist.push(Number(state.finalMix[i].deezerId));
    }
    DZ.player.playTracks(state.finalPlaylist);
  },
  [types.ADDNEXTTRACK](state, payload) {
    var a = DZ.player.getCurrentIndex();   
    state.nextTrack= [];
    var x = state.finalPlaylist.indexOf(Number(state.currentTrack));
    state.nextTrack.push(Number(state.currentTrack));
    for(var i=x+1; i<state.finalPlaylist.length; i++){
      state.nextTrack.push(state.finalPlaylist[i])
    }
    for(var y=0; y<x; y++) {
      state.nextTrack.push(Number(state.finalPlaylist[y]))
    }
    state.finalPlaylist = [];
    Array.prototype.push.apply(state.finalPlaylist, state.nextTrack);   
    DZ.Event.subscribe('track_end', function(evt_name){
      console.log("fini");
       DZ.player.playTracks(state.finalPlaylist);
    });
  }
}

export default {
  state,
  mutations
}