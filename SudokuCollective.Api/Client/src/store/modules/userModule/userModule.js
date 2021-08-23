import {
  UPDATE_SELECTED_USER,
  UPDATE_USERS,
  REMOVE_USER,
  REMOVE_USERS,
  REPLACE_USER,
} from "./mutation-types";

import User from "@/models/user";

const userModule = {
  namespaced: true,

  state: () => ({
    selectedUser: new User(),
    users: [],
  }),

  mutations: {
    [UPDATE_SELECTED_USER](state, user) {
      state.selectedUser = user;
    },
    [UPDATE_USERS](state, users) {
      users.forEach((user) => {
        const index = state.users.indexOf(user.id);
        if (index !== -1) {
          state.users.splice(index, 1, user);
        } else {
          state.users.push(user);
        }
      });
    },
    [REMOVE_USER](state, user) {
      const index = state.users.indexOf(user.id);
      if (index !== -1) {
        state.users.splice(index, 1);
      }
    },
    [REMOVE_USERS](state) {
      state.users = [];
    },
    [REPLACE_USER](state, user) {
      const index = state.users.indexOf(user.id);
      if (index !== -1) {
        state.users.splice(index, 1, user);
      } else {
        state.users.push(user);
      }
    },
  },

  actions: {
    updateSelectedUser({ commit }, user) {
      commit(UPDATE_SELECTED_USER, user);
    },
    updateUsers({ commit }, users) {
      commit(UPDATE_USERS, users);
    },
    removeUser({ commit }, user) {
      commit(REMOVE_USER, user);
    },
    removeUsers({ commit }) {
      commit(REMOVE_USERS);
    },
    replaceUser({ commit }, user) {
      commit(REPLACE_USER, user);
    },
  },

  getters: {
    getSelectedUser: (state) => {
      return state.selectedUser;
    },
    getUserById: (state) => (id) => {
      return state.users.find((user) => user.id === id);
    },
    getUsers: (state) => {
      return state.users;
    },
  },
};

export default userModule;
