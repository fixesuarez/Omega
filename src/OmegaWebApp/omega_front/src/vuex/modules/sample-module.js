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
  },
  [types.SENDCRITERIAS](state, payload) {
    state.criterias = payload;
  },
  [types.ENABLECRITERIAS](state, payload) {
    state.enabledCriterias = payload;
  },
  [types.ADDMOOD](state, payload) {
    state.moods.push.payload;
  }
}

export default {
  state,
  mutations
}