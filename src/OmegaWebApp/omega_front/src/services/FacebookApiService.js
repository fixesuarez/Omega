import { getAsync, postAsync } from '../helpers/apiHelper'
import { uploadAsync } from '../helpers/uploadHelper'
import AuthService from './AuthService'

const endpoint = "/api/EventGroup";

class FacebookApiService {
    constructor() {

    }

    async getFacebookEvents() {
        return await getAsync(endpoint, 'RetrieveUserEvents', AuthService.accessToken);
    }
    async getFacebookGroups() {
        return await getAsync(endpoint, 'RetrieveUserGroups', AuthService.accessToken);
    }
    async createEvent(event) {
        return await uploadAsync(endpoint, 'CreateEvent', AuthService.accessToken, event);
    }
    async AddMember(member) {
        return await postAsync(endpoint, 'AddMember', AuthService.accessToken, member);
    }
}

export default new FacebookApiService()