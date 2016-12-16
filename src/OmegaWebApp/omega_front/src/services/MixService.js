import { getAsync } from '../helpers/apiHelper'
import { postAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Mix";

class MixService {
    constructor() {

    }

    async mix(mixToMix) {
        return await postAsync(endpoint, 'MixPlaylist', AuthService.accessToken, mixToMix);
    }
}

export default new MixService()