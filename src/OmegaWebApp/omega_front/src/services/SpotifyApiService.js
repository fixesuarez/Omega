import { getAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Playlist";

class SpotifyApiService {
    constructor() {

    }

    async getSpotifyPlaylist() {
        return await getAsync(endpoint, 'Playlist', AuthService.accessToken);
    }
}

export default new SpotifyApiService()