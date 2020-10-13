import Vue from "vue";
import Vuex from "vuex";

import appConfigModule from "./modules/appConfigModule/appConfigModule";
import userModule from "./modules/userModule/userModule";

Vue.use(Vuex);

export default new Vuex.Store({
    state: {},
    mutations: {},
    actions: {},
    modules: {
        appConfigModule,
        userModule,
    }
});
