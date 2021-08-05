<template>
  <div>
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">
            Review App: {{ app.name }}
          </v-card-title>
          <hr class="title-spacer" />
          <AppProfileWidget
            :app="app" />
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
  </div>
  
</template>

<style>

</style>

<script>
/* eslint-disable no-unused-vars */
import { mapGetters } from "vuex";
import App from "@/models/app";
import AppProfileWidget from "@/components/widgets/AppProfileWidget";

export default {
  name: "ReviewAppWidget",
  components: {
    AppProfileWidget
  },
  data: () => ({
    app: new App()
  }),
  computed: {
    ...mapGetters("appModule", ["getSelectedApp"]),
  },
  watch: {
    "$store.state.appModule.selectedApp": {
      handler: function (val, oldVal) {
        this.$data.app = this.getSelectedApp;
      }
    },
  },
  created() {
    this.$data.app = this.getSelectedApp;
  }

}
</script>
