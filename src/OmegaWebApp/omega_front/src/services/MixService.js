import { getAsync } from '../helpers/apiHelper'
import { postAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Mix";

class MixService {
    constructor() {

    }

    async mix() {
        return await postAsync(endpoint, 'Mix', AuthService.accessToken);
    }
}

export default new MixService()