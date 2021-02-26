import { 
  UPDATE_APP, 
  UPDATE_APPS,
  REMOVE_APPS,
} from "./mutation-types";

import App from "@/models/app";

const appModule = {
  namespaced: true,

  state: () => ({
    App: new App(),
    Apps: []
  }),

  mutations: {
    [UPDATE_APP](state, app) {
      state.App = app;
    },
    [UPDATE_APPS](state, apps) {
      apps.forEach(app => state.Apps.push(app));
    },
    [REMOVE_APPS](state) {
      state.Apps = [];
    },
  },

  actions: {
    updateApp({ commit }, app) {
      commit(UPDATE_APP, app);
    },
    updateApps({ commit }, apps) {
      commit(UPDATE_APPS, apps);
    },
    removeApps({ commit }) {
      commit(REMOVE_APPS);
    },
  },

  getters: {
    getApp: (state) => {
      return state.App;
    },
    getAppById: (state) => (id) => {
      return state.Apps.find(app => app.id === id)
    },
    getApps: (state) => {
      return state.Apps;
    },
  },
};

export default appModule;
