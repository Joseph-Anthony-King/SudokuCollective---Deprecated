import { baseURLConfirmationService } from "../../../services/baseURLConfirmation.service";
import { CONFIRM_BASE_URL, UPDATE_API_MESSAGE } from "./mutation-types";

const appConfigModule = {

    namespaced: true,

    state: () => ({
        baseURL: "",
        apiMessage: ""
    }),

    mutations: {
        [CONFIRM_BASE_URL](state, confirmedURL) {
            state.baseURL = confirmedURL;
        },
        [UPDATE_API_MESSAGE](state, updatedAPIMessage) {
            state.apiMessage = updatedAPIMessage;
        }
    },

    actions: {
        async confirmBaseURL({ commit }, baseURL) {
            const response = await baseURLConfirmationService.confirm(baseURL);
            commit(CONFIRM_BASE_URL, response.url);
            commit(UPDATE_API_MESSAGE, response.message)
        }
    },

    getters: {
        getBaseURL: state => { return state.baseURL },
        getAPIMessage: state => { return state.apiMessage }
    }
}

export default appConfigModule;