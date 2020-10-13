import Vue from "vue";
import Vuex from "vuex";

import appConfig from "./modules/appConfig/appConfig"

Vue.use(Vuex);

export default new Vuex.Store({
    state: {},
    mutations: {},
    actions: {},
    modules: {
        appConfig
    }
});
