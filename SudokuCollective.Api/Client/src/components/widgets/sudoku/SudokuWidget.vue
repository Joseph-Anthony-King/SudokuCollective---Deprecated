<template>
  <v-container fluid>
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <div class="center">
            <v-combobox
              class="combo"
              v-model="selectedMode"
              :items="modes"
              item-text="key"
              label="Please make a selection"
              single-line
              outlined
              dense
              return-object
            ></v-combobox>
          </div>
          <hr class="title-spacer" />
          <MatrixWidget />
          <div class="title-spacer" />
          <v-card-title class="justify-center" v-if="playGame"
            >Difficulty Level</v-card-title
          >
          <v-combobox
            v-if="playGame"
            class="combo"
            v-model="difficulty"
            :items="difficulties"
            item-text="displayName"
            label="Select Difficulty"
            single-line
            outlined
            dense
            return-object
          ></v-combobox>
        </v-container>
      </v-card-text>
    </v-card>
    <hr v-if="!getProcessing" />
    <v-card elevation="6" class="mx-auto" v-if="!getProcessing">
      <v-card-title class="justify-center">Available Actions</v-card-title>
      <v-card-actions>
        <v-container>
          <v-row dense>
            <v-col v-if="playGame">
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="createGame"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Create Game
                  </v-btn>
                </template>
                <span>Create Game</span>
              </v-tooltip>
            </v-col>
            <v-col v-if="playGame">
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="checkGame"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Check Game
                  </v-btn>
                </template>
                <span>Check Game</span>
              </v-tooltip>
            </v-col>
            <v-col v-if="playGame">
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="resetGame"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Reset Game
                  </v-btn>
                </template>
                <span>Reset Game</span>
              </v-tooltip>
            </v-col>
            <v-col>
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="solve"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Solve
                  </v-btn>
                </template>
                <span>Obtain the solution</span>
              </v-tooltip>
            </v-col>
            <v-col>
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="clear"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Clear
                  </v-btn>
                </template>
                <span>Clear the sudoku puzzle</span>
              </v-tooltip>
            </v-col>
          </v-row>
        </v-container>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<style scoped>
.center {
  margin: auto;
  width: 50%;
}
.combo {
  min-width: 15em;
  max-width: 25em;
  margin: auto;
  width: 50%;
  font-size: small;
  text-align: center;
}
</style>

<script>
/* eslint-disable no-useless-escape, no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { gamesProvider } from "@/providers/gamesProvider";
import { solutionsProvider } from "@/providers/solutionsProvider";
import MatrixWidget from "@/components/widgets/sudoku/MatrixWidget";
import SolveModel from "@/models/viewModels/solveModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

export default {
  name: "SudokuWidget",
  components: {
    MatrixWidget,
  },
  data: () => ({
    solutionPending: true,
    playGame: null,
    modes: [
      { key: "Play Game", value: true },
      { key: "Solve Sudoku Puzzle", value: false },
    ],
    difficulties: [],
    selectedMode: null,
    difficulty: null,
  }),
  methods: {
    ...mapActions("settingsModule", ["updateProcessing"]),
    ...mapActions("sudokuModule", [
      "initializePuzzle",
      "initializeGame",
      "initializeInitialGame",
      "updatePuzzle",
      "updateGame",
      "updateInitialGame",
      "updateSelectedDifficulty",
      "updatePlayGame",
    ]),

    async createGame() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            this.updateProcessing(true);

            try {
              const response = await gamesProvider.createGame(
                this.$data.difficulty.difficultyLevel
              );

              if (response.isSuccess) {
                this.updateGame(response.game);
                this.updateInitialGame(response.game);
                showToast(
                  this,
                  ToastMethods["success"],
                  response.message,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  response.message,
                  defaultToastOptions()
                );
              }
            } catch (error) {
              showToast(
                this,
                ToastMethods["error"],
                error,
                defaultToastOptions()
              );
            } finally {
              this.updateProcessing(false);
            }
          },
        },

        {
          text: "No",
          onClick: (e, toastObject) => {
            toastObject.goAway(0);
          },
        },
      ];

      showToast(
        this,
        ToastMethods["show"],
        `Are you sure you want to create a new ${this.$data.difficulty.displayName.toLowerCase()} game?`,
        actionToastOptions(action, "mode_edit")
      );
    },

    async checkGame() {
      this.updateProcessing(true);

      try {
        const response = await gamesProvider.checkGame(this.getGame);

        if (response.isSuccess) {
          showToast(
            this,
            ToastMethods["success"],
            response.message,
            defaultToastOptions()
          );
        } else {
          showToast(
            this,
            ToastMethods["error"],
            response.message,
            defaultToastOptions()
          );
        }
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      } finally {
        this.updateProcessing(false);
      }
    },

    resetGame() {
      this.updateGame(this.getInitialGame);
    },

    async solve() {
      if (this.$data.playGame) {
        const action = [
          {
            text: "Yes",
            onClick: async (e, toastObject) => {
              toastObject.goAway(0);

              await this.submitPuzzle();
            },
          },

          {
            text: "No",
            onClick: (e, toastObject) => {
              toastObject.goAway(0);
            },
          },
        ];

        showToast(
          this,
          ToastMethods["show"],
          `Are you sure you want to submit this game for a solution?`,
          actionToastOptions(action, "mode_edit")
        );
      } else {
        await this.submitPuzzle();
      }
    },

    async submitPuzzle() {
      this.updateProcessing(true);
      var matrix = [];

      for (var i = 0; i < 9; i++) {
        let line;

        if (this.$data.playGame) {
          line = this.getGame.slice(i * 9, i * 9 + 9);
        } else {
          line = this.getPuzzle.slice(i * 9, i * 9 + 9);
        }

        let row = [];

        line.forEach((number) => {
          if (number === "") {
            row.push(0);
          } else {
            row.push(parseInt(number));
          }
        });

        matrix.push(row);
      }

      try {
        const response = await solutionsProvider.solve(new SolveModel(matrix));

        if (response.status === 200) {
          if (this.$data.playGame) {
            this.updateGame(response.matrix);
          } else {
            this.updatePuzzle(response.matrix);
          }

          this.$data.solutionPending = false;
          this.$forceUpdate();
          showToast(
            this,
            ToastMethods["success"],
            response.message,
            defaultToastOptions()
          );
        } else if (response.status === 404) {
          showToast(
            this,
            ToastMethods["error"],
            response.message.substring(17),
            defaultToastOptions()
          );
        } else if (response.status === 400) {
          showToast(
            this,
            ToastMethods["error"],
            response.message,
            defaultToastOptions()
          );
        } else {
          showToast(
            this,
            ToastMethods["error"],
            response.data.message,
            defaultToastOptions()
          );
        }
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      } finally {
        this.updateProcessing(false);
      }
    },

    clear() {
      if (this.$data.playGame) {
        this.initializeGame();
        this.initializeInitialGame();
      } else {
        this.initializePuzzle();
      }
      this.$data.solutionPending = true;

      this.updateDifficulty();
    },

    assignData(data) {
      this.$data.select = data;
      this.$data.playGame = this.$data.select.value;
    },

    updateDifficulty() {
      this.$data.difficulties = this.getDifficulties;

      if (this.getSelectedDifficulty !== null) {
        this.$data.difficulty = this.getSelectedDifficulty;
      } else {
        this.$data.difficulty = this.$data.difficulties[0];
        this.updateSelectedDifficulty(this.$data.difficulty);
      }
    },
  },
  computed: {
    ...mapGetters("sudokuModule", [
      "getPuzzle",
      "getGame",
      "getInitialGame",
      "getDifficulties",
      "getSelectedDifficulty",
      "getPlayGame",
    ]),
    ...mapGetters("settingsModule", ["getProcessing"]),
  },
  watch: {
    selectedMode: {
      handler: function (val, oldVal) {
        this.assignData(val);
      },
    },
    playGame: {
      handler: function (val, oldVal) {
        this.updatePlayGame(this.$data.playGame);
      },
    },
    difficulty: {
      handler: function (val, oldVal) {
        this.updateSelectedDifficulty(this.$data.difficulty);
      },
    },
  },
  mounted() {
    if (this.getPlayGame) {
      this.$data.selectedMode = this.$data.modes[0];
    } else {
      this.$data.selectedMode = this.$data.modes[1];
    }

    this.updateDifficulty();
  },
};
</script>
