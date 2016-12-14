import { getAsync } from '../helpers/apiHelper'
import { postAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Mix";

class MixService {
    constructor() {

    }

    async mix() {
        return await getAsync(endpoint, 'MixPlaylist', AuthService.accessToken);
    }
}

export default new MixService()