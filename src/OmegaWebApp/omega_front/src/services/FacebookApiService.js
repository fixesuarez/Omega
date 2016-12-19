import { getAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/EventGroup";

class FacebookApiService {
    constructor() {

    }

    async getFacebookEvents() {
        return await getAsync(endpoint, 'RetrieveUserEvents', AuthService.accessToken);
    }
    async getFacebookGroups() {
        return await getAsync(endpoint, 'Events', AuthService.accessToken);
    }
}

export default new FacebookApiService()