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
    <h3>Password Reset Page Setup</h3>
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
  name: "CustomPasswordResetSetupGauge",
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
      let apps = [];

      if (!this.$data.user.isSuperUser && this.$data.selectedApp.id === 0) {
        apps = this.getUsersApps;
      } else {
        apps = this.getApps;
      }

      let notSetUpApps;
      let ratio = 0;
      let result;

      if (
        this.$data.user.isSuperUser ||
        (!this.$data.user.isSuperUser && this.$data.selectedApp.id === 0)
      ) {
        notSetUpApps = _.filter(apps, function (app) {
          return (
            app.disableCustomUrls ||
            (!app.disableCustomUrls && app.customPasswordResetAction === "")
          );
        });

        if (apps.length > 0) {
          ratio = (notSetUpApps.length / apps.length) * 100;
        }

        result = ratio.toFixed(0) + "%";
      } else {
        if (
          !this.$data.selectedApp.disableCustomUrls &&
          this.$data.selectedApp.customPasswordResetAction !== ""
        ) {
          result = "Set up";
        } else {
          result = "Not Set up";
        }
      }

      return result;
    },

    legend() {
      let apps = [];

      if (!this.$data.user.isSuperUser) {
        apps = this.getUsersApps;
      } else {
        apps = this.getApps;
      }

      let notSetUpApps = [];

      if (apps.length > 0) {
        notSetUpApps = _.filter(apps, function (app) {
          return (
            app.disableCustomUrls ||
            (!app.disableCustomUrls && app.customPasswordResetAction === "")
          );
        });
      }

      let app;
      let result;

      if (
        this.$data.user.isSuperUser ||
        (!this.$data.user.isSuperUser && this.$data.selectedApp.id === 0)
      ) {
        if (apps.length > 0 && notSetUpApps.length > 0) {
          app = notSetUpApps.length === 1 ? " App needs " : " Apps need ";
          return notSetUpApps.length + app + " a password reset action";
        } else if (apps.length > 0 && notSetUpApps.length === 0) {
          app = apps.length === 1 ? " App has " : " Apps have ";
          return apps.length + app + "a password action";
        } else {
          return "No Apps Created";
        }
      } else {
        if (
          !this.$data.selectedApp.disableCustomUrls &&
          this.$data.selectedApp.customPasswordResetAction !== ""
        ) {
          result = "Set Up";
        } else {
          result = "Not Set Up";
        }
      }

      return result;
    },

    isApplicable() {
      let apps = [];

      if (this.$data.selectedApp.id === 0) {
        apps = this.getUsersApps;
      } else {
        apps.push(this.$data.selectedApp);
      }

      let notSetUp = [];

      if (apps.length > 0) {
        notSetUp = _.filter(apps, function (app) {
          return (app.disableCustomUrls ||
            (!app.disableCustomUrls && app.customPasswordResetAction === ""));
        });
      }

      if (notSetUp.length > 0) {
        return true;
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
