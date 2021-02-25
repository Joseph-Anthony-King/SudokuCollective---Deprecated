import { UPDATE_ADMIN_APP } from "./mutation-types";

import App from "@/models/app";

const adminAppModule = {
  namespaced: true,

  state: () => ({
    AdminApp: new App(),
  }),

  mutations: {
    [UPDATE_ADMIN_APP](state, app) {
      state.AdminApp = app;
    },
  },

  actions: {
    updateAdminApp({ commit }, app) {
      commit(UPDATE_ADMIN_APP, app);
    },
  },

  getters: {
    getAdminApp: (state) => {
      return state.AdminApp;
    },
  },
};

export default adminAppModule;