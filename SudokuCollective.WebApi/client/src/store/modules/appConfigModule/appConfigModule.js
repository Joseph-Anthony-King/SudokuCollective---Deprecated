import { baseURLConfirmationService } from "../../../services/baseURLConfirmation.service";
import {
    CONFIRM_BASE_URL,
    UPDATE_API_MESSAGE,
    UPDATE_AUTH_TOKEN,
    UPDATE_REQUESTOR_ID
} from "./mutation-types";

const appConfigModule = {

    namespaced: true,

    state: () => ({
        baseURL: "",
        apiMessage: "",
        authToken: "",
        requestorId: 0
    }),

    mutations: {
        [CONFIRM_BASE_URL](state, confirmedURL) {
            state.baseURL = confirmedURL;
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
        async confirmBaseURL({ commit }, baseURL) {
            const response = await baseURLConfirmationService.confirm(baseURL);
            commit(CONFIRM_BASE_URL, response.url);
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
        getBaseURL: state => { return state.baseURL },
        getAPIMessage: state => { return state.apiMessage },
        getAuthToken: state => { return state.authToken },
        getRequestorId: state => { return state.requestorId }
    }
}

export default appConfigModule;