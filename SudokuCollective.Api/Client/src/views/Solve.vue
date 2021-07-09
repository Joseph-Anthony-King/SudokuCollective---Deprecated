<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <SolvePage />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import SolvePage from "@/components/pages/SolvePage.vue";
export default {
  name: "Solve",
  components: {
    SolvePage,
  },
};
</script>