import { 
  UPDATE_SELECTED_APP, 
  UPDATE_APPS,
  REMOVE_APPS,
} from "./mutation-types";

import App from "@/models/app";

const appModule = {
  namespaced: true,

  state: () => ({
    SelectedApp: new App(),
    Apps: []
  }),

  mutations: {
    [UPDATE_SELECTED_APP](state, app) {
      state.SelectedApp = app;
    },
    [UPDATE_APPS](state, apps) {
      apps.forEach(app => state.Apps.push(app));
    },
    [REMOVE_APPS](state) {
      state.Apps = [];
    },
  },

  actions: {
    updateSelectedApp({ commit }, app) {
      commit(UPDATE_SELECTED_APP, app);
    },
    updateApps({ commit }, apps) {
      commit(UPDATE_APPS, apps);
    },
    removeApps({ commit }) {
      commit(REMOVE_APPS);
    },
  },

  getters: {
    getSelectedApp: (state) => {
      return state.SelectedApp;
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
