import { apiURLConfirmationService } from "../../../services/apiURLConfirmationService/apiURLConfirmation.service";
import {
    CONFIRM_API_URL,
    UPDATE_API_MESSAGE,
    UPDATE_AUTH_TOKEN,
    UPDATE_REQUESTOR_ID
} from "./mutation-types";

const appSettingsModule = {

    namespaced: true,

    state: () => ({
        apiURL: "",
        apiMessage: "",
        authToken: "",
        requestorId: 0
    }),

    mutations: {
        [CONFIRM_API_URL](state, confirmedURL) {
            state.apiURL = confirmedURL;
        },
        [UPDATE_API_MESSAGE](state, updatedAPIMessage) {
            state.apiMessage = updatedAPIMessage;
        },
        [UPDATE_AUTH_TOKEN](state, token) {
            state.authToken = token;
        },
        [UPDATE_REQUESTOR_ID](state, requestorId) {
            state.requestorId = requestorId;
        }
    },

    actions: {
        async confirmBaseURL({ commit }, apiURL) {
            const response = await apiURLConfirmationService.confirm(apiURL);
            commit(CONFIRM_API_URL, response.url);
            commit(UPDATE_API_MESSAGE, response.message);
        },

        updateAuthToken({ commit }, token) {
            commit(UPDATE_AUTH_TOKEN, token);
        },

        updateRequestorId({ commit }, id) {
            commit(UPDATE_REQUESTOR_ID, id);
        }
    },

    getters: {
        getApiURL: state => { return state.apiURL },
        getAPIMessage: state => { return state.apiMessage },
        getAuthToken: state => { return state.authToken },
        getRequestorId: state => { return state.requestorId }
    }
}

export default appSettingsModule;