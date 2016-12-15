import * as types from './mutation-types'

async function wrapAsyncApiCall(commit, apiCall, rethrowError) {
    commit(types.SET_IS_LOADING, true);

    let result = null;

    try {
        return await apiCall();
    }
    catch (error) {
        commit(types.ERROR_HAPPENED, `${error.status}: ${error.responseText || error.statusText}`);
        
        if(rethrowError) throw error;
    }
    finally {
        commit(types.SET_IS_LOADING, false);
    }
}

export async function requestAsync({ commit }, action, rethrowError) {
    return await wrapAsyncApiCall(commit, action, rethrowError);
}

export const increment = ({commit}) => {
    commit(types.INCREMENT)
}

export const decrement = ({commit}) => {
    commit(types.DECREMENT)
}

export const update = ({commit}) => {
    commit(types.UPDATE)
}

export const add = ({commit}, payload) => {
    commit(types.ADD, payload)
}

export const addText = ({commit}, payload) => {
    commit(types.ADDTEXT, payload)
}

export const makeActive = ({commit}, payload) => {
  
    commit(types.MAKEACTIVE, payload)
}

export const showPlaylistHelperModal = ({commit}, payload) => {
  
    commit(types.SHOWPLAYLISTHELPERMODAL, payload)
}

export const showEventModal = ({commit}, payload) => {
  
    commit(types.SHOWEVENTMODAL, payload)
}

export const showMoodsModal = ({commit}, payload) => {
  
    commit(types.SHOWMOODSMODAL, payload)
}

export const sendMoods = ({commit}, payload) => {
  
    commit(types.SENDMOODS, payload)
}

export const sendCriterias = ({commit}, payload) => {
  
    commit(types.SENDCRITERIAS, payload)
}

export const enableCriterias = ({commit}, payload) => {
  
    commit(types.ENABLECRITERIAS, payload)
}

export const addMood = ({commit}, payload) => {
  
    commit(types.ADDMOOD, payload)
}

export const authenticate = ({commit}, payload) => {
  
    commit(types.AUTHENTICATE, payload)
}

export const setCurrentMood = ({commit}, payload) => {
  
    commit(types.SETCURRENTMOOD, payload)
}

export const setCurrentPlaylist = ({commit}, payload) => {
  
    commit(types.SETCURRENTPLAYLIST, payload)
}

export const selectPlaylist = ({commit}, payload) => {
  
    commit(types.SELECTPLAYLIST, payload)
}

export const sendPlaylists = ({commit}, payload) => {
  
    commit(types.SENDPLAYLISTS, payload)
}

export const insertMood = ({commit}, payload) => {
    
    commit(types.INSERTMOOD, payload)
}

export const mix = ({commit}, payload) => {
    
    commit(types.MIX, payload)
}