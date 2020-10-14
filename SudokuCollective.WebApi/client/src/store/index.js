import Vue from "vue";
import Vuex from "vuex";
import createPersistedState from "vuex-persistedstate";

import appConfigModule from "./modules/appConfigModule/appConfigModule";
import userModule from "./modules/userModule/userModule";

Vue.use(Vuex);

const store = new Vuex.Store({
    state: {},
    mutations: {},
    actions: {},
    modules: {
        appConfigModule,
        userModule,
    },
    plugins: [createPersistedState()]
});

export default store