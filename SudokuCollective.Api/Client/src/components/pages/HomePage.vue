<template>
  <v-container fluid>
    <div class="d-flex ma-12 pa-12 justify-center" v-if="app.name === ''" >
      <v-progress-circular
        indeterminate
        color="primary"
        :size="100"
        :width="10"
        class="progress-circular"
      ></v-progress-circular>
    </div>
    <v-card elevation="6" class="mx-auto" v-if="app.name !== ''">
      <v-row class="text-center home-banner">
        <v-col cols="12">
          <v-img src="/images/banner.jpg" height="500" />
          <h1 class="text-center centered-welcome-message text-padding">Welcome to {{ app.name }}</h1>
          <v-img src="/images/logo.png" class="my-3 centered-logo" contain height="200" />
        </v-col>
      </v-row>
      <v-row>
        <p class="motto text-center text-padding">
          Code... Create... Inspire...
        </p>
        <p class="description text-center text-padding">
          Sudoku Collective is a project that serves as a ready made Web API
          that you can use to learn client side technologies. The API is
          documented so you can create your own client app which can fully
          integrate with the API. My particular implementation will include
          console and Vue apps.
        </p>
      </v-row>
    </v-card>
  </v-container>
</template>

<style scoped>
.text-padding {
  padding: 20px 70px 20px 70px;
}
.home-banner {
    position: relative;
    margin-bottom: 0;
    border: 0;
    border-color: transparent;
}
.centered-welcome-message {
    position: absolute;
    top: 10%;
    left: 0;
    width: 100%;
    color: white;
    text-shadow: 2px 2px var(--v-secondary);
}
.centered-logo {
  position: absolute;
  top: 50%;
  left: 0;
  width: 100%;
  filter: drop-shadow(2px 2px 2px var(--v-secondary));
}
.motto {
  margin: auto;
  font-style: italic;
  color: var(--v-secondary);
  font-size: 2em;
}
.description {
  color: var(--v-secondary);
  font-size: 1em;
}
</style>

<script>
/* eslint-disable no-unused-vars */
import { mapGetters } from "vuex";
import App from "@/models/app";

export default {
  name: "HomePage",
  data: () => ({
    app: new App(),
  }),
  computed: {
    ...mapGetters("settingsModule", ["getApp"])
  },
  watch: {
    "$store.state.settingsModule.app": {
      handler: function(val, oldVal) {
        this.$data.app = new App(this.getApp);
      }
    },
  },
  created() {
    this.$data.app = new App(this.getApp);
  },
};
</script>
