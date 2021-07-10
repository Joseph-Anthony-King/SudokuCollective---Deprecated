import _ from 'lodash';
import { 
  UPDATE_SELECTED_APP, 
  UPDATE_APPS,
  UPDATE_REGISTERED_APPS,
  REMOVE_APP,
  REMOVE_APPS,
  REMOVE_REGISTERED_APPS,
  REPLACE_APP,
} from "./mutation-types";

import App from "@/models/app";

const appModule = {
  namespaced: true,

  state: () => ({
    selectedApp: new App(),
    apps: [],
    registeredApps: [],
  }),

  mutations: {
    [UPDATE_SELECTED_APP](state, app) {
      state.selectedApp = new App(app);
    },
    [UPDATE_APPS](state, apps) {
      apps.forEach((app) => { 
        const index = _.findIndex(state.apps, { id: app.id });
        if (index !== -1) {
          state.apps.splice(index, 1, app);
        } else {
          state.apps.push(app);
        }
      });
    },
    [UPDATE_REGISTERED_APPS](state, registeredApps) {
      registeredApps.forEach((app) => { 
        const index = _.findIndex(state.registeredApps, { id: app.id });
        if (index !== -1) {
          state.registeredApps.splice(index, 1, app);
        } else {
          state.registeredApps.push(app);
        }
      });
    },
    [REMOVE_APP](state, app) {
      const index = _.findIndex(state.apps, { id: app.id });
      if (index !== -1) {
        state.apps.splice(index, 1);
      }
    },
    [REMOVE_APPS](state) {
      state.apps = [];
    },
    [REMOVE_REGISTERED_APPS](state) {
      state.registeredApps = [];
    },
    [REPLACE_APP](state, app) {
      const index = _.findIndex(state.apps, { id: app.id });
      if (index !== -1) {
        state.apps.splice(index, 1, app);
      } else {
        state.apps.push(app);
      }
    },
  },

  actions: {
    updateSelectedApp({ commit }, app) {
      commit(UPDATE_SELECTED_APP, app);
    },
    updateApps({ commit }, apps) {
      commit(UPDATE_APPS, apps);
    },
    updateRegisteredApps({ commit }, registeredApps) {
      commit(UPDATE_REGISTERED_APPS, registeredApps)
    },
    removeApp({ commit }, app) {
      commit(REMOVE_APP, app);
    },
    removeApps({ commit }) {
      commit(REMOVE_APPS);
    },
    removeRegisteredApps({ commit }) {
      commit(REMOVE_REGISTERED_APPS)
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
    getRegisteredApps: (state) => {
      return state.registeredApps
    },
  },
};

export default appModule;
