import User from "@/models/user";
import App from "@/models/app";
import {
  CONFIRM_API_URL,
  UPDATE_AUTH_TOKEN,
  EXPIRE_AUTH_TOKEN,
  UPDATE_TOAST_DURATION,
  UPDATE_APP,
  UPDATE_USER,
  UPDATE_USERNAME,
} from "./mutation-types";

const settingsModule = {
  namespaced: true,

  state: () => ({
    apiURL: "",
    authToken: "",
    authTokenExpired: false,
    toastDuration: 500,
    app: new App(),
    user: new User(),
    userName: "",
  }),

  mutations: {
    [CONFIRM_API_URL](state, confirmedURL) {
      state.apiURL = confirmedURL;
    },
    [UPDATE_AUTH_TOKEN](state, token) {
      state.authToken = token;
      state.authTokenExpired = false;
    },
    [EXPIRE_AUTH_TOKEN](state) {
      state.authTokenExpired = true;
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
    [UPDATE_USERNAME](state, username) {
      state.userName = username;
    },
  },

  actions: {
    async confirmBaseURL({ commit }, apiURL) {
      commit(CONFIRM_API_URL, apiURL);
    },
    updateAuthToken({ commit }, token) {
      commit(UPDATE_AUTH_TOKEN, token);
    },
    expireAuthToken({ commit }) {
      commit(EXPIRE_AUTH_TOKEN);
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
    updateUserName({ commit }, username) {
      commit(UPDATE_USERNAME, username);
    },
  },

  getters: {
    getApiURL: (state) => {
      return state.apiURL;
    },
    getAuthToken: (state) => {
      return state.authToken;
    },
    getAuthTokenExpired: (state) => {
      return state.authTokenExpired;
    },
    getRequestorId: (state) => {
      return state.user.id;
    },
    getToastDuration: (state) => {
      return state.toastDuration;
    },
    getApp: (state) => {
      return state.app;
    },
    getAppId: (state) => {
      return state.app.id;
    },
    getLicense: (state) => {
      return state.app.license;
    },
    getUser: (state) => {
      return state.user;
    },
    getAppName: (state) => {
      return state.app.name;
    },
    getUserName: (state) => {
      return state.userName;
    },
  },
};

export default settingsModule;
