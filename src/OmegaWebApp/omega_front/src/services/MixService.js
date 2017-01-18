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
    async getMix() {
        return await getAsync(endpoint, 'RetrieveMix', AuthService.accessToken);
    }
    async createMix(mix) {
        return await postAsync(endpoint, 'CreateMix', AuthService.accessToken, mix);
    }
}

export default new MixService()