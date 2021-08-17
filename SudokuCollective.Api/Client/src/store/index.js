import Vue from "vue";
import Vuex from "vuex";
import createPersistedState from "vuex-persistedstate";

import appModule from "./modules/appModule/appModule";
import settingsModule from "./modules/settingsModule/settingsModule";
import userModule from "./modules/userModule/userModule";
import sudokuModule from "./modules/sudokuModule/sudokuModule";

Vue.use(Vuex);

const store = new Vuex.Store({
  state: {},
  mutations: {},
  actions: {},
  modules: {
    appModule,
    settingsModule,
    userModule,
    sudokuModule,
  },
  plugins: [createPersistedState()],
});

export default store;
