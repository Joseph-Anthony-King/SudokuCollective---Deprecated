import { 
  UPDATE_SELECTED_APP, 
  UPDATE_APPS,
  REMOVE_APPS,
  REPLACE_APP,
} from "./mutation-types";

import App from "@/models/app";

const appModule = {
  namespaced: true,

  state: () => ({
    selectedApp: new App(),
    apps: []
  }),

  mutations: {
    [UPDATE_SELECTED_APP](state, app) {
      state.selectedApp = new App(app);
    },
    [UPDATE_APPS](state, apps) {
      apps.forEach(app => state.apps.push(app));
    },
    [REMOVE_APPS](state) {
      state.apps = [];
    },
    [REPLACE_APP](state, updatedApp) {
      let a;
      for (let app of state.apps) {
        if (app.id === updatedApp.id) {
          a = app;
        }
      }
      const index = state.apps.indexOf(a);
      state.apps.splice(index, 1);
      state.apps.push(updatedApp);
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
    replaceApp({ commit }, app) {
      commit(REPLACE_APP, app);
    },
  },

  getters: {
    getSelectedApp: (state) => {
      return state.selectedApp;
    },
    getAppById: (state) => (id) => {
      return state.apps.find(app => app.id === id)
    },
    getApps: (state) => {
      return state.apps;
    },
  },
};

export default appModule;
