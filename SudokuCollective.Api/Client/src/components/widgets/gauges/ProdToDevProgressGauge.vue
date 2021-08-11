<template>
  <div class="text-center">
    <v-progress-circular
      :rotate="90"
      :size="310"
      :width="30"
      :value="value"
      :color="isApplicable ? 'error' : 'success'"
    >
      {{ value }}
    </v-progress-circular>
    <h3>Development to Production</h3>
    <h3>{{ legend }}</h3>
    <br />
  </div>
</template>

<style scoped>
.v-progress-circular {
  margin: 2rem;
}
</style>

<script>
/* eslint-disable no-unused-vars */
import _ from "lodash";
import { mapGetters } from "vuex";
import App from "@/models/app";
import User from "@/models/user";

export default {
  name: "ProdToDevProgressGauge",
  data: () => ({
    user: new User(),
    selectedApp: new App(),
  }),
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", [
      "getUsersSelectedApp",
      "getUsersApps",
      "getApps",
    ]),

    value() {
      let apps;

      if (!this.$data.user.isSuperUser) {
        apps = this.getUsersApps;
      } else {
        apps = this.getApps;
      }

      let result;

      if (this.$data.selectedApp.id === 0 || this.$data.user.isSuperUser && this.$data.selectedApp.ownerId !== this.$data.user.id) {
        let totalApps = apps.length;
        let totalDevelopment = _.filter(apps, function (app) {
          return app.inDevelopment;
        });
        let ratio = 0;

        if (apps.length > 0) {
          ratio = (totalDevelopment.length / totalApps) * 100;
        }

        result = ratio.toFixed(0) + "%";
      } else {
        if (this.$data.selectedApp.inDevelopment) {
          result = "In Development";
        } else {
          result = "In Production";
        }
      }

      return result;
    },

    legend() {
      let apps;

      if (!this.$data.user.isSuperUser) {
        apps = this.getUsersApps;
      } else {
        apps = this.getApps;
      }

      let totalDevelopment = _.filter(apps, function (app) {
        return app.inDevelopment;
      });

      let result;

      if (this.$data.selectedApp.id === 0 || this.$data.user.isSuperUser && this.$data.selectedApp.ownerId !== this.$data.user.id) {
        if (apps.length > 0 && totalDevelopment.length > 0) {
          return (
            "Apps in Development: " +
            totalDevelopment.length +
            " Apps in Production: " +
            (apps.length - totalDevelopment.length)
          );
        } else if (apps.length > 0 && totalDevelopment.length === 0) {
          const app = apps.length === 1 ? "App is " : "All Apps are ";
          result = app + "in Production";
        } else {
          result = "No Apps Created";
        }
      } else {
        if (this.$data.selectedApp.inDevelopment) {
          result = "App is in Development";
        } else {
          result = "App is in Production";
        }
      }

      return result;
    },

    isApplicable() {
      if (this.$data.user.isSuperUser && this.$data.selectedApp.ownerId !== this.$data.user.id) {
        return false;
      } else if (
        this.$data.selectedApp.id !== 0 &&
        this.$data.selectedApp.inDevelopment
      ) {
        return true;
      } else if (
        this.$data.selectedApp.id !== 0 &&
        !this.$data.selectedApp.inDevelopment
      ) {
        return false;
      } else {
        return false;
      }
    },
  },
  watch: {
    "$store.state.appModule.usersSelectedApp": {
      handler: function (val, oldVal) {
        this.$data.selectedApp = this.getUsersSelectedApp;
      },
    },
  },
  created() {
    this.$data.user = this.getUser;
  },
};
</script>
