import * as types from '../mutation-types'

const state = {
  count: 0,
  nb: 3,
  choice: 0,
  active: '',
  text: ''
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
    state.active = payload.active ;
  },
  [types.ADDTEXT](state, payload) {
    state.text = payload.text ;
  }

}

export default {
  state,
  mutations
}