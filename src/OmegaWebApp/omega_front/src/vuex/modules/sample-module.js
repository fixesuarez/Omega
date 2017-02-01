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
  groupModalActive: false,  
  moodsModalActive: false,
  memberModalActive: false,
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
  toPlayer: [],
  nextTrack: [],
  moodToInsert: '',
  mixToInsert: '',
  memberToInsert: '',
  member: '',
  eventToInsert:'',
  groupToInsert:'',
  mixToMix: {AmbianceName: '', AllPlaylists: ''},
  identity: false,
  track: '',
  trackInPlayer: ''
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
  [types.SHOWGROUPMODAL](state, payload) {
    state.groupModalActive = payload;
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
  [types.SHOWMEMBERMODAL](state, payload) {
    state.memberModalActive = payload;
  },

//DATA SENDING

  [types.SENDMOODS](state, payload) {
    state.moods = payload;
  },
  [types.SENDMIX](state, payload) {
    state.mix = payload;
  },
  [types.SENDMEMBER](state, payload) {
    state.member = payload;
  },
  [types.SENDEVENTS](state, payload) {
    state.events = payload;
    state.loading = false;         
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
    state.loading = false;     
  },
  [types.SENDMIX](state, payload) {
    state.finalPlaylist = [];
    state.finalMix = payload;
    state.currentTrack = state.finalMix[0];

    for(var i=0; i<state.finalMix.length; i++){
      state.finalPlaylist.push(Number(state.finalMix[i].deezerId));
    }
    DZ.player.playTracks(state.finalPlaylist); 
    state.loading = false;
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
  [types.SETLOADING](state, payload) {
    state.loading = payload;
  },
  [types.SETPLAYINGTRACK](state, payload) {
    state.trackInPlayer = payload;
  },

//INSERTS

  [types.INSERTMOOD](state, payload) {
    state.moodToInsert= payload;
  },
  [types.INSERTMIX](state, payload) {
    state.mixToInsert = payload;
  },
  [types.INSERTMEMBER](state, payload) {
    state.memberToInsert = payload;
  },
  [types.INSERTEVENT](state, payload) {
    state.eventToInsert = payload;
  },
  [types.INSERTGROUP](state, payload) {
    state.groupToInsert = payload;
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
    // state.currentTrack = DZ.player.getCurrentTrack().id;
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
    state.toPlayer = [];
    DZ.player.playTracks();
    state.finalMix = [];
    var a = state.allMix.indexOf(payload);
    for(var i=0; i<state.allMix[a].parsedPlaylist.length; i++){
      state.finalMix.push(state.allMix[a].parsedPlaylist[i])
    }
    state.currentTrack = state.finalMix[0];

    for(var i=0; i<state.finalMix.length; i++){
      state.toPlayer.push(Number(state.finalMix[i].deezerId));
    }
    DZ.player.playTracks(state.toPlayer);
    DZ.player.changeTrackOrder(state.toPlayer);
  },
  [types.ADDNEXTTRACK](state, payload) {
    if(payload.deezerId !== DZ.player.getCurrentTrack().id) {
      state.toPlayer = [];
      state.finalMix.splice((state.finalMix.indexOf(payload)), 1);
      var playingTrack = DZ.player.getCurrentTrack().id;
      for(var z = 0; z < state.finalMix.length; z++) {
        if(state.finalMix[z].deezerId == playingTrack) {
          var result = state.finalMix.indexOf(state.finalMix[z]);
          state.finalMix.splice(result+1, 0, payload)
        }
      }
      for(var a = 0; a < state.finalMix.length; a++) {
        state.toPlayer.push(Number(state.finalMix[a].deezerId));
      }
      DZ.player.changeTrackOrder(state.toPlayer);
    }

    // DZ.Event.subscribe('current_track', function(track, evt_name){
    //   var playingTrack = DZ.player.getCurrentTrack().id;
    //   playingTrack = '"'+ playingTrack +'"';
    //   console.log(playingTrack);
    //   state.trackInPlayer = playingTrack;
    // });
  }
}

export default {
  state,
  mutations
}