import { 
  UPDATE_APP, 
  UPDATE_APPS,
  REMOVE_APPS,
  ADD_APP,
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
    [ADD_APP](state, app) {
      state.Apps.push(app);
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
    addApp({ commit }, app) {
      commit(ADD_APP, app);
    },
  },

  getters: {
    getApp: (state) => {
      return state.App;
    },
    getApps: (state) => {
      return state.Apps;
    },
  },
};

export default appModule;
