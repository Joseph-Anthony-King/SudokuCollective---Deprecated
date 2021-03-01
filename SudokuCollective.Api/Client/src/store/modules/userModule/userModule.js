import { 
  UPDATE_SELECTED_USER, 
  UPDATE_USERS,
  REMOVE_USERS,
} from "./mutation-types";

import User from "@/models/user";

const userModule = {
  namespaced: true,

  state: () => ({
    SelectedUser: new User(),
    Users: []
  }),

  mutations: {
    [UPDATE_SELECTED_USER](state, app) {
      state.SelectedApp = app;
    },
    [UPDATE_USERS](state, apps) {
      apps.forEach(app => state.Apps.push(app));
    },
    [REMOVE_USERS](state) {
      state.Apps = [];
    },
  },

  actions: {
    updateSelectedApp({ commit }, app) {
      commit(UPDATE_SELECTED_USER, app);
    },
    updateApps({ commit }, apps) {
      commit(UPDATE_USERS, apps);
    },
    removeApps({ commit }) {
      commit(REMOVE_USERS);
    },
  },

  getters: {
    getSelectedApp: (state) => {
      return state.SelectedUser;
    },
    getAppById: (state) => (id) => {
      return state.Users.find(user => user.id === id)
    },
    getApps: (state) => {
      return state.Users;
    },
  },
};

export default userModule;
