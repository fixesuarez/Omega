import { getAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Playlist";

class PlaylistApiService {
    constructor() {

    }
    
    async getPlaylists() {
        return await getAsync(endpoint, 'Playlists', AuthService.accessToken);
    }
}

export default new PlaylistApiService()