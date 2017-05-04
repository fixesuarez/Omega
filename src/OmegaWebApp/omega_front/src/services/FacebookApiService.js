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
        return await postAsync(endpoint, 'CreateEvent', AuthService.accessToken, event);
    }
    async deleteEvent(id) {
        return await postAsync(endpoint, 'DeleteEventGroup', AuthService.accessToken, id);
    }
    async createGroup(group) {
        return await postAsync(endpoint, 'CreateGroup', AuthService.accessToken, group);
    }
    async uploadEventCover(cover, eventGuid, eventName) {
        return await uploadAsync(endpoint, 'UploadEventGroupCover/'+eventGuid+'/'+eventName, AuthService.accessToken, cover);
    }
    async uploadGroupCover(cover, groupGuid, groupName) {
        return await uploadAsync(endpoint, 'UploadEventGroupCover/'+groupGuid+'/'+groupName, AuthService.accessToken, cover);
    }
    async AddMember(member) {
        return await postAsync(endpoint, 'AddMember', AuthService.accessToken, member);
    }
}

export default new FacebookApiService()