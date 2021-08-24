import {
  INITIALIZE_PUZZLE,
  UPDATE_PUZZLE,
  REMOVE_PUZZLE,
  INITIALIZE_GAME,
  UPDATE_GAME,
  REMOVE_GAME,
  INITIALIZE_INITIAL_GAME,
  UPDATE_INITIAL_GAME,
  REMOVE_INITIAL_GAME,
  UPDATE_DIFFICULTIES,
  REMOVE_DIFFICULTIES,
  UPDATE_PLAYGAME,
  UPDATE_SELECTED_DIFFICULTY,
  REMOVE_SELECTED_DIFFICULTY,
} from "./mutation-types";

const sudokuModule = {
  namespaced: true,

  state: () => ({
    puzzle: [],
    game: [],
    initialGame: [],
    difficulties: [],
    selectedDifficulty: null,
    playGame: true,
  }),

  mutations: {
    [INITIALIZE_PUZZLE](state) {
      state.puzzle = [];
      for (var i = 0; i < 81; i++) {
        state.puzzle.push("");
      }
    },
    [UPDATE_PUZZLE](state, puzzle) {
      let newPuzzle = [81];
      for (let i = 0; i < puzzle.length; i++) {
        newPuzzle[i] = puzzle[i];
      }
      state.puzzle = newPuzzle;
    },
    [REMOVE_PUZZLE](state) {
      state.puzzle = [];
    },
    [INITIALIZE_GAME](state) {
      state.game = [];
      for (var i = 0; i < 81; i++) {
        state.game.push("");
      }
    },
    [UPDATE_GAME](state, game) {
      let newGame = [81];
      for (let i = 0; i < game.length; i++) {
        newGame[i] = game[i];
      }
      state.game = newGame;
    },
    [REMOVE_GAME](state) {
      state.game = [];
    },
    [INITIALIZE_INITIAL_GAME](state) {
      state.initialGame = [];
      for (var i = 0; i < 81; i++) {
        state.initialGame.push("");
      }
    },
    [UPDATE_INITIAL_GAME](state, game) {
      let newGame = [81];
      for (let i = 0; i < game.length; i++) {
        newGame[i] = game[i];
      }
      state.initialGame = newGame;
    },
    [REMOVE_INITIAL_GAME](state) {
      state.initialGame = [];
    },
    [UPDATE_DIFFICULTIES](state, difficulties) {
      state.difficulties = [];
      state.difficulties = difficulties;
    },
    [REMOVE_DIFFICULTIES](state) {
      state.difficulties = [];
    },
    [UPDATE_SELECTED_DIFFICULTY](state, difficulty) {
      state.selectedDifficulty = difficulty;
    },
    [REMOVE_SELECTED_DIFFICULTY](state) {
      state.selectedDifficulty = null;
    },
    [UPDATE_PLAYGAME](state, playGame) {
      state.playGame = playGame;
    },
  },

  actions: {
    initializePuzzle({ commit }) {
      commit(INITIALIZE_PUZZLE);
    },

    updatePuzzle({ commit }, puzzle) {
      commit(UPDATE_PUZZLE, puzzle);
    },

    removePuzzle({ commit }) {
      commit(REMOVE_PUZZLE);
    },

    initializeGame({ commit }) {
      commit(INITIALIZE_GAME);
    },

    updateGame({ commit }, game) {
      commit(UPDATE_GAME, game);
    },

    removeGame({ commit }) {
      commit(REMOVE_GAME);
    },

    initializeInitialGame({ commit }) {
      commit(INITIALIZE_INITIAL_GAME);
    },

    updateInitialGame({ commit }, game) {
      commit(UPDATE_INITIAL_GAME, game);
    },

    removeInitialGame({ commit }) {
      commit(REMOVE_INITIAL_GAME);
    },

    updateDifficulties({ commit }, difficulties) {
      commit(UPDATE_DIFFICULTIES, difficulties);
    },

    removeDifficulties({ commit }) {
      commit(REMOVE_DIFFICULTIES);
    },

    updateSelectedDifficulty({ commit }, difficulty) {
      commit(UPDATE_SELECTED_DIFFICULTY, difficulty);
    },

    removeSelectedDifficulty({ commit }) {
      commit(REMOVE_SELECTED_DIFFICULTY);
    },

    updatePlayGame({ commit }, playGame) {
      commit(UPDATE_PLAYGAME, playGame);
    },
  },

  getters: {
    getPuzzle: (state) => {
      return state.puzzle;
    },

    getGame: (state) => {
      return state.game;
    },

    getInitialGame: (state) => {
      return state.initialGame;
    },

    getDifficulties: (state) => {
      return state.difficulties;
    },

    getSelectedDifficulty: (state) => {
      return state.selectedDifficulty;
    },

    getPlayGame: (state) => {
      return state.playGame;
    },
  },
};

export default sudokuModule;
