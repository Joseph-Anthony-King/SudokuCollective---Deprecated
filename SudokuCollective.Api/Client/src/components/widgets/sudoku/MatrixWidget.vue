<template>
  <div>
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
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
        :background-color="applyEvenRegionStyling(index) ? 'primary' : '#fff'"
        :color="applyEvenRegionStyling(index) ? '#fff' : 'primary'"
        hide-details
        @blur="validateEntry(index)"
      ></v-text-field>
    </v-row>
  </div>
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
import { mapActions } from "vuex";
import { mapGetters } from "vuex";

export default {
  name: "MatrixWidget",
  data: () => ({
    sudokuMatrix: [],
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
    ...mapActions("sudokuModule", ["updatePuzzle", "updateGame"]),

    validateEntry(index) {
      var number = parseInt(this.$data.sudokuMatrix[index]);

      if (number < 1 || number > 9) {
        this.$data.sudokuMatrix[index] = "";
        this.$forceUpdate();
      }

      if (this.getPlayGame) {
        this.updateGame(this.$data.sudokuMatrix);
      } else {
        this.updatePuzzle(this.$data.sudokuMatrix);
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
  computed: {
    ...mapGetters("sudokuModule", ["getPuzzle", "getGame", "getPlayGame"]),
  },
  watch: {
    "$store.state.sudokuModule.puzzle": {
      handler: function (val, oldVal) {
        this.$data.sudokuMatrix = this.getPuzzle;
        this.$forceUpdate();
      },
    },
    "$store.state.sudokuModule.game": {
      handler: function (val, oldVal) {
        this.$data.sudokuMatrix = this.getGame;
        this.$forceUpdate();
      },
    },
    "$store.state.sudokuModule.playGame": {
      handler: function (val, oldVal) {
        if (this.getPlayGame) {
          this.$data.sudokuMatrix = this.getGame;
        } else {
          this.$data.sudokuMatrix = this.getPuzzle;
        }
      },
    },
  },
  created() {
    if (this.getPlayGame) {
      this.$data.sudokuMatrix = this.getGame;
    } else {
      this.$data.sudokuMatrix = this.getPuzzle;
    }
  },
};
</script>
