import User from "@/models/user";
import App from "@/models/app";
import {
  CONFIRM_API_URL,
  UPDATE_AUTH_TOKEN,
  UPDATE_TOAST_DURATION,
  UPDATE_APP,
  UPDATE_USER
} from "./mutation-types";

const settingsModule = {
  namespaced: true,

  state: () => ({
    apiURL: "",
    authToken: "",
    toastDuration: 500,
    app: new App(),
    user: new User(),
  }),

  mutations: {
    [CONFIRM_API_URL](state, confirmedURL) {
      state.apiURL = confirmedURL;
    },
    [UPDATE_AUTH_TOKEN](state, token) {
      state.authToken = token;
    },
    [UPDATE_TOAST_DURATION](state, duration) {
      state.toastDuration = duration;
    },
    [UPDATE_APP](state, app) {
      state.app = app;
    },
    [UPDATE_USER](state, user) {
      state.user = user;
    },
  },

  actions: {
    async confirmBaseURL({ commit }, apiURL) {
      commit(CONFIRM_API_URL, apiURL);
    },
    updateAuthToken({ commit }, token) {
      commit(UPDATE_AUTH_TOKEN, token);
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
      return state.user.id;
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
      return state.user;
    },
  },
};

export default settingsModule;
