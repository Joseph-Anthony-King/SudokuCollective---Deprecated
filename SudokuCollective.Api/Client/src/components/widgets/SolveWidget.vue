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
          <v-card-title class="justify-center" v-if="solutionPending"
            >Sudoku Puzzle in need of Solution</v-card-title
          >
          <v-card-title class="justify-center" v-if="!solutionPending"
            >Following Solution has been Found</v-card-title
          >
          <hr class="title-spacer" />
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [0, 1, 2, 3, 4, 5, 6, 7, 8]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [9, 10, 11, 12, 13, 14, 15, 16, 17]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [18, 19, 20, 21, 22, 23, 24, 25, 26]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [27, 28, 29, 30, 31, 32, 33, 34, 35]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [36, 37, 38, 39, 40, 41, 42, 43, 44]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [45, 46, 47, 48, 49, 50, 51, 52, 53]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [54, 55, 56, 57, 58, 59, 60, 61, 62]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [63, 64, 65, 66, 67, 68, 69, 70, 71]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-for="index in [72, 73, 74, 75, 76, 77, 78, 79, 80]"
              :key="index"
              v-model="sudokuMatrix[index]"
              :class="
                applyEvenRegionStyling(index)
                  ? 'centered-input text-white ma-0 pa-0'
                  : 'centered-input text-secondary ma-0 pa-0'
              "
              outlined
              type="number"
              min="1"
              max="9"
              :background-color="
                applyEvenRegionStyling(index) ? 'primary' : '#fff'
              "
              :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
              hide-details
              @blur="validateEntry(index)"
            ></v-text-field>
          </v-row>
        </v-container>
      </v-card-text>
    </v-card>
    <hr v-if="!processing" />
    <v-card elevation="6" class="mx-auto" v-if="!processing">
      <v-card-title class="justify-center">Available Actions</v-card-title>
      <v-card-actions>
        <v-container>
          <v-row dense>
            <v-col>
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="submit"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Submit
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
.v-row {
  flex-shrink: auto;
}
/* Galaxy Fold... folded */
@media only screen and (max-width: 319px) {
  .v-text-field {
    max-width: 27px;
    max-height: 54px;
    font-size: xx-small;
  }
}
/* Moto G4, Galaxy S5, iPhone 5/SE */
@media only screen and (min-width: 320px) and (max-width: 373px) {
  .v-text-field {
    max-width: 32px;
    max-height: 54px;
    font-size: medium;
  }
}
/* iPhone 6/7/8, iPhone X, iPhone 12 */
@media only screen and (min-width: 374px) and (max-width: 409px) {
  .v-text-field {
    max-width: 38px;
    max-height: 54px;
    font-size: x-large;
    font-weight: bold;
  }
}
/* Pixel 2, Pixel 2 XL, Galaxy Note 10+ */
@media only screen and (min-width: 410px) and (max-width: 481px) {
  .v-text-field {
    max-width: 42px;
    max-height: 54px;
    font-size: x-large;
    font-weight: bold;
  }
}
/* iPhone 6/7/8 Plus, Surface Duo - Folded */
@media only screen and (min-width: 482px) and (max-width: 642px) {
  .v-text-field {
    max-width: 50px;
    max-height: 54px;
    font-size: x-large;
    font-weight: bold;
  }
}
/* desktop */
@media only screen and (min-width: 643px) {
  .v-text-field {
    max-width: 66px;
    max-height: 86px;
    font-size: xx-large;
    font-weight: bold;
  }
}
.centered-input >>> input {
  text-align: center;
}
.text-secondary >>> input {
  color: var(--v-secondary);
}
.text-white >>> input {
  color: white;
}
</style>

<script>
/* eslint-disable no-useless-escape, no-unused-vars */
import _ from "lodash";
import { solutionService } from "@/services/solutionService/solutionService";
import SolveModel from "@/models/viewModels/solveModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "SolveWidget",
  data: () => ({
    sudokuMatrix: [],
    processing: false,
    solutionPending: true,
    evenRegionIndexes: [
      3,
      4,
      5,
      12,
      13,
      14,
      21,
      22,
      23,
      27,
      28,
      29,
      36,
      37,
      38,
      45,
      46,
      47,
      33,
      34,
      35,
      42,
      43,
      44,
      51,
      52,
      53,
      57,
      58,
      59,
      66,
      67,
      68,
      75,
      76,
      77,
    ],
  }),
  methods: {
    async submit() {
      this.$data.processing = true;
      var matrix = [];

      for (var i = 0; i < 9; i++) {
        var line = this.$data.sudokuMatrix.slice(i * 9, i * 9 + 9);
        var row = [];

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
        const response = await solutionService.postSolve(
          new SolveModel(matrix)
        );

        if (response.status === 200) {
          for (var j = 0; j < 81; j++) {
            this.$data.sudokuMatrix[j] = response.data.solution.solutionList[
              j
            ].toString();
          }
          this.$data.solutionPending = false;
          this.$forceUpdate();
        } else if (response.status === 404) {
          showToast(
            this,
            ToastMethods["error"],
            response.data.message.substring(17),
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
      this.$data.sudokuMatrix = [];
      for (var i = 0; i < 81; i++) {
        this.$data.sudokuMatrix.push("");
      }
      this.$data.solutionPending = true;
      this.$forceUpdate();
    },

    validateEntry(index) {
      var number = parseInt(this.$data.sudokuMatrix[index]);

      if (number < 1 || number > 9) {
        this.$data.sudokuMatrix[index] = "";
        this.$forceUpdate();
      }
    },

    applyEvenRegionStyling(index) {
      var result = _.indexOf(this.$data.evenRegionIndexes, index);

      if (result !== -1) {
        return true;
      } else {
        return false;
      }
    },
  },
  created() {
    for (var i = 0; i < 81; i++) {
      this.$data.sudokuMatrix.push("");
    }
  },
};
</script>
