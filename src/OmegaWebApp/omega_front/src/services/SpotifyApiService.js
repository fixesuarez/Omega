import { getAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Spotify";

class SpotifyApiService {
    constructor() {

    }

    async getSpotifyPlaylist() {
        return await getAsync(endpoint, 'Playlists', AuthService.accessToken);
    }
}

export default new SpotifyApiService()