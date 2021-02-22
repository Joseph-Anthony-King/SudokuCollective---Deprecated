import { UPDATE_USER } from "./mutation-types";

import User from "../../../models/user";

const userModule = {
  namespaced: true,

  state: () => ({
    User: new User(),
  }),

  mutations: {
    [UPDATE_USER](state, user) {
      state.User = user;
    },
  },

  actions: {
    updateUser({ commit }, user) {
      commit(UPDATE_USER, user);
    },
  },

  getters: {
    getUser: (state) => {
      return state.User;
    },
  },
};

export default userModule;
