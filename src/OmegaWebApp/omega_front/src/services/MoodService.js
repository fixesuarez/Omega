import { getAsync } from '../helpers/apiHelper'
import { postAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/AmbianceController";

class MoodService {
    constructor() {

    }

    async getMoods() {
        return await getAsync(endpoint, 'Moods', AuthService.accessToken);
    }
    async createMood() {
        return await postAsync(endpoint, 'InsertAmbiance', AuthService.accessToken);
    }
}

export default new MoodService()