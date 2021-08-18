<template>
  <v-container fluid>
    <v-card elevation="6" class="mx-auto ma-0" v-if="processing">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center"
            >Sudoku puzzle is being processed, please do not navigate
            away...</v-card-title
          >
          <v-row cols="12">
            <v-progress-circular
              indeterminate
              color="primary"
              :size="100"
              :width="10"
              class="progress-circular"
            ></v-progress-circular>
          </v-row>
        </v-container>
      </v-card-text>
    </v-card>
    <v-card elevation="6" class="mx-auto" v-if="!processing">
      <v-card-text>
        <v-container fluid>
          <div class="center">
            <v-combobox
              class="combo"
              v-model="select"
              :items="items"
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
    <hr v-if="!processing" />
    <v-card elevation="6" class="mx-auto" v-if="!processing">
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
import _ from "lodash";
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
    processing: false,
    solutionPending: true,
    playGame: null,
    items: [
      { key: "Play Game", value: true },
      { key: "Solve Sudoku Puzzle", value: false },
    ],
    difficulties: [],
    select: null,
    difficulty: null,
  }),
  methods: {
    ...mapActions("sudokuModule", [
      "initializePuzzle",
      "initializeGame",
      "updatePuzzle",
      "updateGame",
      "updateSelectedDifficulty",
      "updatePlayGame",
    ]),

    async createGame() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await gamesProvider.createGame(
                this.$data.difficulty.difficultyLevel
              );

              if (response.success) {
                this.updateGame(response.game);
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
      try {
        const response = await gamesProvider.checkGame(this.getGame);

        if (response.success) {
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
      }
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
        this.$data.processing = true;
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

          console.log(response);

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
            const errorKeys = Object.keys(response.data.errors);
            var errorMessage = "Submission failed with the following errors: ";

            if (errorKeys.length === 1) {
              errorKeys.forEach((key, index) => {
                errorMessage = errorMessage + `${response.data.errors[key]}`;
              });
            } else {
              errorKeys.forEach((key, index) => {
                if (index !== errorKeys.length - 1) {
                  errorMessage =
                    errorMessage + `${response.data.errors[key]}` + " & ";
                } else {
                  errorMessage = errorMessage + `${response.data.errors[key]}`;
                }
              });
            }

            showToast(
              this,
              ToastMethods["error"],
              errorMessage,
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
        }

        this.$data.processing = false;
    },

    clear() {
      if (this.$data.playGame) {
        this.initializeGame();
      } else {
        this.initializePuzzle();
      }
      this.$data.solutionPending = true;
    },

    assignData(value) {
      this.$data.select = value;
      this.$data.playGame = this.$data.select.value;
    },
  },
  computed: {
    ...mapGetters("sudokuModule", [
      "getPuzzle",
      "getGame",
      "getDifficulties",
      "getSelectedDifficulty",
    ]),
  },
  watch: {
    select: {
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
  created() {
    this.assignData(this.$data.items[0]);

    this.$data.difficulties = this.getDifficulties;

    if (this.getSelectedDifficulty !== null) {
      this.$data.difficulty = this.getSelectedDifficulty;
    } else {
      this.$data.difficulty = this.$data.difficulties[0];
      this.updateSelectedDifficulty(this.$data.difficulty);
    }
  },
};
</script>
