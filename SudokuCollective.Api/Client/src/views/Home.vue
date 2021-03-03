<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <HomePage />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import HomePage from "@/components/pages/HomePage.vue";

export default {
  name: "Home",
  components: {
    HomePage,
  },
};
</script>
