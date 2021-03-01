<template>
  <v-container fluid>
    <v-card elevation="6" class="mx-auto" v-if="app.name === ''">
      <v-card-text class="text-center"> loading... </v-card-text>
    </v-card>
    <v-card elevation="6" class="mx-auto" v-if="app.name !== ''">
      <h1 class="text-center text-padding">Welcome to {{ app.name }}</h1>
      <v-row class="text-center">
        <v-col cols="12">
          <v-img src="/logo.png" class="my-3" contain height="200" />
        </v-col>
      </v-row>
      <v-row>
        <h3 class="text-center text-padding">
          Sudoku Collective is a project that serves as a ready made Web API
          that you can use to learn client side technologies. The API is
          documented so you can create your own client app which can fully
          integrate with the API. My particular implementation will include
          console and Vue apps.
        </h3>
      </v-row>
    </v-card>
  </v-container>
</template>

<style scoped>
.text-padding {
  padding: 50px 70px 50px 70px;
}
</style>

<script>
import App from "@/models/app";

export default {
  name: "HomeForm",
  data: () => ({
    app: new App(),
  }),
  watch: {
    "$store.state.settingsModule.app": function () {
      this.$data.app.clone(this.$store.getters["settingsModule/getApp"]);
      console.log("app widget app updated:", this.$data.app);
    },
  },
  created() {
    this.$data.app.clone(this.$store.getters["settingsModule/getApp"]);
  },
};
</script>
