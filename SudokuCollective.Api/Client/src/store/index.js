import Vue from "vue";
import Vuex from "vuex";
import createPersistedState from "vuex-persistedstate";

import adminAppModule from "./modules/adminAppModule/adminAppModule";
import appSettingsModule from "./modules/appSettingsModule/appSettingsModule";
import userModule from "./modules/userModule/userModule";

Vue.use(Vuex);

const store = new Vuex.Store({
  state: {},
  mutations: {},
  actions: {},
  modules: {
    adminAppModule,
    appSettingsModule,
    userModule,
  },
  plugins: [createPersistedState()],
});

export default store;
