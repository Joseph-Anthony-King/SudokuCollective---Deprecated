import Vue from "vue";
import Vuex from "vuex";
import createPersistedState from "vuex-persistedstate";

import adminAppModule from "./modules/adminAppModule/adminAppModule";
import settingsModule from "./modules/settingsModule/settingsModule";
import userModule from "./modules/userModule/userModule";

Vue.use(Vuex);

const store = new Vuex.Store({
  state: {},
  mutations: {},
  actions: {},
  modules: {
    adminAppModule,
    settingsModule,
    userModule,
  },
  plugins: [createPersistedState()],
});

export default store;
