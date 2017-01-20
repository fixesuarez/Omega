import { getAsync } from '../helpers/apiHelper'
import { postAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/Ambiance";

class MoodService {
    constructor() {

    }

    async getMoods() {
        return await getAsync(endpoint, 'RetrieveAllUserAmbiance', AuthService.accessToken);
    }
    async createMood(mood) {
        return await postAsync(endpoint, 'InsertAmbiance', AuthService.accessToken, mood);
    }
    async deleteMood(mood) {
        return await postAsync(endpoint, 'DeleteAmbiance', AuthService.accessToken, mood);
    }
}

export default new MoodService()