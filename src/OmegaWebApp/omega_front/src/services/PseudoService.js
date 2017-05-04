import { getAsync } from '../helpers/apiHelper'
import { postAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/User";

class PseudoService {
    constructor() {

    }
    async getPseudo() {
        return await getAsync(endpoint, 'RetrievePseudo', AuthService.accessToken);
    }
    async SavePseudo(pseudo) {
        return await postAsync(endpoint, 'UpdatePseudo', AuthService.accessToken, pseudo);
    }
}

export default new PseudoService()