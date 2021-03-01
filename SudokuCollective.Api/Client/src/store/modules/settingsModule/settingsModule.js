import { apiURLConfirmationService } from "../../../services/apiURLConfirmationService/apiURLConfirmation.service";
import User from "@/models/user";
import App from "@/models/app";
import {
  CONFIRM_API_URL,
  UPDATE_AUTH_TOKEN,
  UPDATE_REQUESTOR_ID,
  UPDATE_TOAST_DURATION,
  UPDATE_APP,
  UPDATE_USER
} from "./mutation-types";

const settingsModule = {
  namespaced: true,

  state: () => ({
    apiURL: "",
    authToken: "",
    requestorId: 0,
    toastDuration: 500,
    app: new App(),
    User: new User(),
  }),

  mutations: {
    [CONFIRM_API_URL](state, confirmedURL) {
      state.apiURL = confirmedURL;
    },
    [UPDATE_AUTH_TOKEN](state, token) {
      state.authToken = token;
    },
    [UPDATE_REQUESTOR_ID](state, requestorId) {
      state.requestorId = requestorId;
    },
    [UPDATE_TOAST_DURATION](state, duration) {
      state.toastDuration = duration;
    },
    [UPDATE_APP](state, app) {
      state.app = app;
    },
    [UPDATE_USER](state, user) {
      state.User = user;
    },
  },

  actions: {
    async confirmBaseURL({ commit }, apiURL) {
      const response = await apiURLConfirmationService.confirm(apiURL);
      commit(CONFIRM_API_URL, response.url);
    },
    updateAuthToken({ commit }, token) {
      commit(UPDATE_AUTH_TOKEN, token);
    },
    updateRequestorId({ commit }, id) {
      commit(UPDATE_REQUESTOR_ID, id);
    },
    updateToastDuration({ commit }, duration) {
      commit(UPDATE_TOAST_DURATION, duration);
    },    
    updateApp({ commit }, app) {
      commit(UPDATE_APP, app);
    },
    updateUser({ commit }, user) {
      commit(UPDATE_USER, user);
    },
  },

  getters: {
    getApiURL: (state) => {
      return state.apiURL;
    },
    getAuthToken: (state) => {
      return state.authToken;
    },
    getRequestorId: (state) => {
      return state.requestorId;
    },
    getToastDuration: (state) => {
      return state.toastDuration;
    },
    getLicense: (state) => {
      return state.app.license;
    },
    getApp: (state) => {
      return state.app;
    },
    getUser: (state) => {
      return state.User;
    },
  },
};

export default settingsModule;
