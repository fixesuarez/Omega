import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/spotify";

class SpotifyApiService {
    constructor() {

    }

    async getPlaylistList() {
        return await getAsync(endpoint, 'playlist', AuthService.accessToken);
    }
}

export default new SpotifyApiService()