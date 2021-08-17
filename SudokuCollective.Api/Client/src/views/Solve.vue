<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <SudokuPage />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import SudokuPage from "@/components/pages/SudokuPage.vue";
export default {
  name: "Solve",
  components: {
    SudokuPage,
  },
};
</script>
