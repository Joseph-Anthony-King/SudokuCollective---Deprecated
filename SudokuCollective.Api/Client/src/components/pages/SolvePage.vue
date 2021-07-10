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
              v-model="sudokuMatrix[0]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(0)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[1]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(1)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[2]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(2)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[3]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(3)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[4]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(4)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[5]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(5)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[6]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(6)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[7]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(7)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[8]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(8)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-model="sudokuMatrix[9]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(9)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[10]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(10)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[11]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(11)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[12]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(12)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[13]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(13)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[14]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(14)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[15]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(15)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[16]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(16)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[17]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(17)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-model="sudokuMatrix[18]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(18)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[19]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(19)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[20]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(20)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[21]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(21)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[22]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(22)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[23]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(23)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[24]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(24)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[25]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(25)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[26]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(26)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-model="sudokuMatrix[27]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(27)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[28]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(28)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[29]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(29)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[30]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(30)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[31]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(31)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[32]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(32)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[33]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(33)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[34]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(34)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[35]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(35)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-model="sudokuMatrix[36]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(36)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[37]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(37)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[38]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(38)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[39]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(39)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[40]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(40)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[41]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(41)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[42]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(42)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[43]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(43)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[44]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(44)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-model="sudokuMatrix[45]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(45)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[46]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(46)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[47]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(47)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[48]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(48)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[49]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(49)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[50]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(50)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[51]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(51)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[52]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(52)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[53]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(53)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-model="sudokuMatrix[54]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(54)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[55]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(55)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[56]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(56)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[57]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(57)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[58]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(58)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[59]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(59)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[60]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(60)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[61]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(61)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[62]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(62)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-model="sudokuMatrix[63]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(63)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[64]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(64)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[65]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(65)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[66]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(66)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[67]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(67)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[68]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(68)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[69]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(69)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[70]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(70)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[71]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(71)"
            ></v-text-field>
          </v-row>
          <v-row cols="12" class="ma-0 pa-0 justify-center">
            <v-text-field
              v-model="sudokuMatrix[72]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(72)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[73]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(73)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[74]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(74)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[75]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(75)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[76]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(76)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[77]"
              class="centered-input text-white ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              background-color="primary"
              color="#fff"
              hide-details
              @blur="validateEntry(77)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[78]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(78)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[79]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(79)"
            ></v-text-field>
            <v-text-field
              v-model="sudokuMatrix[80]"
              class="centered-input text-secondary ma-0 pa-0"
              outlined
              type="number"
              min="1"
              max="9"
              hide-details
              @blur="validateEntry(80)"
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
@media only screen and (max-width: 280px) {
  .v-text-field {
    max-width: 27px;
    max-height: 54px;
    font-size: xx-small;
  }
}
/* iPhone 5/SE */
@media only screen and (min-width: 281px) and (max-width: 320px) {
  .v-text-field {
    max-width: 30px;
    max-height: 54px;
    font-size: small;
  }
}
/* Moto G4, Galaxy S5 */
@media only screen and (min-width: 321px) and (max-width: 360px) {
  .v-text-field {
    max-width: 36px;
    max-height: 54px;
    font-size: medium;
  }
}
/* iPhone 6/7/8, iPhone X, iPhone 12 */
@media only screen and (min-width: 361px) and (max-width: 390px) {
  .v-text-field {
    max-width: 38px;
    max-height: 54px;
    font-size: x-large;
    font-weight: bold;
  }
}
/* Pixel 2, Pixel 2 XL, Galaxy Note 10+ */
@media only screen and (min-width: 391px) and (max-width: 412px) {
  .v-text-field {
    max-width: 42px;
    max-height: 54px;
    font-size: x-large;
    font-weight: bold;
  }
}
/* iPhone 6/7/8 Plus, Surface Duo - Folded */
@media only screen and (min-width: 413px) and (max-width: 642px) {
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
import { solutionService } from "@/services/solutionService/solution.service";
import SolveModel from "@/models/viewModels/solveModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "SolvePage",
  data: () => ({
    sudokuMatrix: [],
    processing: false,
    solutionPending: true,
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
            this.$data.sudokuMatrix[j] =
              response.data.solution.solutionList[j].toString();
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
    }
  },
  created() {
    for (var i = 0; i < 81; i++) {
      this.$data.sudokuMatrix.push("");
    }
  },
};
</script>
