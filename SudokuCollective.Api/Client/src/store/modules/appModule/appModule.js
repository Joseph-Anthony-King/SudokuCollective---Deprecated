import {
  UPDATE_USERS_SELECTED_APP,
  UPDATE_USERS_APPS,
  UPDATE_REGISTERED_APPS,
  REPLACE_REGISTERED_APPS,
  REMOVE_USERS_APP,
  REMOVE_USERS_APPS,
  REMOVE_REGISTERED_APPS,
  REPLACE_USERS_APP,
  UPDATE_SELECTED_APP,
  UPDATE_APPS,
  REMOVE_APP,
  REMOVE_APPS,
  REPLACE_APP,
  UPDATE_APP_OWNER,
} from "./mutation-types";

import App from "@/models/app";

const appModule = {
  namespaced: true,

  state: () => ({
    usersSelectedApp: new App(),
    usersApps: [],
    registeredApps: [],
    selectedApp: new App(),
    apps: [],
  }),

  mutations: {
    [UPDATE_USERS_SELECTED_APP](state, app) {
      state.usersSelectedApp = app;
    },
    [UPDATE_USERS_APPS](state, apps) {
      apps.forEach((app) => {
        const index = state.usersApps.indexOf(app.id);
        if (index !== -1) {
          state.usersApps.splice(index, 1, app);
        } else {
          state.usersApps.push(app);
        }
      });
    },
    [UPDATE_REGISTERED_APPS](state, registeredApps) {
      registeredApps.forEach((app) => {
        const index = state.registeredApps.indexOf(app.id);
        if (index !== -1) {
          state.registeredApps.splice(index, 1, app);
        } else {
          state.registeredApps.push(app);
        }
      });
    },
    [REPLACE_REGISTERED_APPS](state, registeredApps) {
      state.registeredApps = registeredApps;
    },
    [REMOVE_USERS_APP](state, app) {
      const index = state.usersApps.indexOf(app.id);
      if (index !== -1) {
        state.usersApps.splice(index, 1);
      }
    },
    [REMOVE_USERS_APPS](state) {
      state.usersApps = [];
    },
    [REMOVE_REGISTERED_APPS](state) {
      state.registeredApps = [];
    },
    [REPLACE_USERS_APP](state, app) {
      const index = state.usersApps.indexOf(app.id);
      if (index !== -1) {
        state.usersApps.splice(index, 1, app);
      } else {
        state.usersApps.push(app);
      }
    },
    [UPDATE_SELECTED_APP](state, app) {
      state.selectedApp = app;
    },
    [UPDATE_APPS](state, apps) {
      apps.forEach((app) => {
        const index = state.apps.indexOf(app.id);
        if (index !== -1) {
          state.apps.splice(index, 1, app);
        } else {
          state.apps.push(app);
        }
      });
    },
    [REMOVE_APP](state, app) {
      const index = state.apps.indexOf(app.id);
      if (index !== -1) {
        state.apps.splice(index, 1);
      }
    },
    [REMOVE_APPS](state) {
      state.apps = [];
    },
    [REPLACE_APP](state, app) {
      const index = state.apps.indexOf(app.id);
      if (index !== -1) {
        state.apps.splice(index, 1, app);
      } else {
        state.apps.push(app);
      }
    },
    [UPDATE_APP_OWNER](state, owner) {
      state.apps.forEach((app) => {
        if (app.ownerId === owner.id) {
          const index = state.apps.indexOf(app.id);
          if (index !== -1) {
            app.owner = owner;
            state.apps.splice(index, 1, app);
          }
        }
      });
    },
  },

  actions: {
    updateUsersSelectedApp({ commit }, app) {
      commit(UPDATE_USERS_SELECTED_APP, app);
    },
    updateUsersApps({ commit }, apps) {
      commit(UPDATE_USERS_APPS, apps);
    },
    updateRegisteredApps({ commit }, registeredApps) {
      commit(UPDATE_REGISTERED_APPS, registeredApps);
    },
    replaceRegisteredApps({ commit }, registeredApps) {
      commit(REPLACE_REGISTERED_APPS, registeredApps);
    },
    removeUsersApp({ commit }, app) {
      commit(REMOVE_USERS_APP, app);
    },
    removeUsersApps({ commit }) {
      commit(REMOVE_USERS_APPS);
    },
    removeRegisteredApps({ commit }) {
      commit(REMOVE_REGISTERED_APPS);
    },
    replaceUsersApp({ commit }, app) {
      commit(REPLACE_USERS_APP, app);
    },
    updateSelectedApp({ commit }, app) {
      commit(UPDATE_SELECTED_APP, app);
    },
    updateApps({ commit }, apps) {
      commit(UPDATE_APPS, apps);
    },
    removeApp({ commit }, app) {
      commit(REMOVE_APP, app);
    },
    removeApps({ commit }) {
      commit(REMOVE_APPS);
    },
    replaceApp({ commit }, app) {
      commit(REPLACE_APP, app);
    },
    updateAppOwner({ commit }, owner) {
      commit(UPDATE_APP_OWNER, owner);
    },
  },

  getters: {
    getUsersSelectedApp: (state) => {
      return state.usersSelectedApp;
    },
    getUsersAppById: (state) => (id) => {
      return state.usersApps.find(app => app.id === id);
    },
    getUsersApps: (state) => {
      return state.usersApps;
    },
    getRegisteredApps: (state) => {
      return state.registeredApps;
    },
    getSelectedApp: (state) => {
      return state.selectedApp;
    },
    getAppById: (state) => (id) => {
      return state.apps.find(app => app.id === id);
    },
    getApps: (state) => {
      return state.apps;
    },
    getAppOwner: (state) => (id) => {
      const app = state.apps.find(app => app.id === id);
      return app.owner;
    },
  },
};

export default appModule;
