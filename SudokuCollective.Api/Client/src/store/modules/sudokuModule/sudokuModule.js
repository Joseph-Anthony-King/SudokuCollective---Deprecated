import {
  INITIALIZE_PUZZLE,
  UPDATE_PUZZLE,
  REMOVE_PUZZLE,
} from "./mutation-types";

const sudokuModule = {
  namespaced: true,

  state: () => ({
    puzzle: [],
  }),

  mutations: {
    [INITIALIZE_PUZZLE](state) {
      state.puzzle = [];
      for (var i = 0; i < 81; i++) {
        state.puzzle.push("");
      }
    },
    [UPDATE_PUZZLE](state, puzzle) {
      state.puzzle = puzzle;
    },
    [REMOVE_PUZZLE](state) {
      state.puzzle = [];
    },
  },

  actions: {
    initializePuzzle({ commit }) {
      console.log("initialize puzzle invoked...");
      commit(INITIALIZE_PUZZLE);
    },

    updatePuzzle({ commit }, puzzle) {
      commit(UPDATE_PUZZLE, puzzle);
    },

    removePuzzle({ commit }) {
      commit(REMOVE_PUZZLE);
    },
  },

  getters: {
    getPuzzle: (state) => {
      return state.puzzle;
    },
  },
};

export default sudokuModule;
