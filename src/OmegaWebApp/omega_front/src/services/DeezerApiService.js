import { getAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Deezer";

class DeezerApiService {
    constructor() {

    }

    async getDeezerPlaylist() {
        return await getAsync(endpoint, 'Playlists', AuthService.accessToken);
    }
}

export default new DeezerApiService()