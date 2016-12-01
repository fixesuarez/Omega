import { getAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Facebook";

class FacebookApiService {
    constructor() {

    }

    async getFacebookGroup() {
        return await getAsync(endpoint, 'Groups', AuthService.accessToken);
    }
    async getFacebookEvent() {
        return await getAsync(endpoint, 'Events', AuthService.accessToken);
    }
}

export default new FacebookApiService()