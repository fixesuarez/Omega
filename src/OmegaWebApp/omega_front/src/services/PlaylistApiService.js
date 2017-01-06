import { getAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Playlist";

class PlaylistApiService {
    constructor() {

    }
    
    async getPlaylists() {
        return await getAsync(endpoint, 'Playlists', AuthService.accessToken);
    }

    async getPlaylistsWithFacebook(id) {
      return await getAsync(endpoint, 'EventOrGroup/'+id, AuthService.accessToken, id);
    }
}

export default new PlaylistApiService()