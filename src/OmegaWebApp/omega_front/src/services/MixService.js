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
        return await getAsync(endpoint, 'RetrieveAllMixUser', AuthService.accessToken);
    }
    async SaveMix(mix) {
        return await postAsync(endpoint, 'CreateMix', AuthService.accessToken, mix);
    }
    async deleteMix(mix) {
        return await postAsync(endpoint, 'DeleteMix', AuthService.accessToken, mix);
    }
}

export default new MixService()