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
    <h3>Email Confirmation Page Setup</h3>
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
import { mapGetters } from "vuex";
import App from "@/models/app";
import User from "@/models/user";

export default {
  name: "CustomEmailConfirmationSetupGauge",
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

      if (!this.$data.user.isSuperUser) {
        apps = this.getUsersApps;
      } else {
        apps = this.getApps;
      }

      let notSetUpApps;
      let ratio = 0;
      let result;

      if (
        (this.$data.user.isSuperUser &&
          this.$data.selectedApp.ownerId !== this.$data.user.id) ||
        (!this.$data.user.isSuperUser && this.$data.selectedApp.id === 0)
      ) {
        /* The following logic is applied if the user if not a super user and an app is not
         * selected OR if the user is a super user and is not review one of their own apps.
         * This then gives the general breakdown for all apps */

        notSetUpApps = apps.filter(
          (app) =>
            app.disableCustomUrls ||
            (!app.disableCustomUrls && app.customEmailConfirmationAction === "")
        );

        if (apps.length > 0) {
          ratio = (notSetUpApps.length / apps.length) * 100;
        }

        result = ratio.toFixed(0) + "%";
      } else {
        /* The following logic is applied if the user if not a super user and an app is
         * selected OR if the user is a super user and is reviewing one of their own apps.
         * This then gives the specific status for the app */

        if (
          !this.$data.selectedApp.disableCustomUrls &&
          this.$data.selectedApp.customEmailConfirmationAction !== ""
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

      let notSetUp = [];

      if (apps.length > 0) {
        notSetUp = apps.filter(
          (app) =>
            app.disableCustomUrls ||
            (!app.disableCustomUrls && app.customEmailConfirmationAction === "")
        );
      }

      let app;
      let result;

      if (
        (this.$data.user.isSuperUser &&
          this.$data.selectedApp.ownerId !== this.$data.user.id) ||
        (!this.$data.user.isSuperUser && this.$data.selectedApp.id === 0)
      ) {
        /* The following logic is applied if the user if not a super user and an app is not
         * selected OR if the user is a super user and is not review one of their own apps.
         * This then gives the general breakdown for all apps */

        if (apps.length > 0 && notSetUp.length > 0) {
          app = notSetUp.length === 1 ? " App needs " : " Apps need ";
          result = notSetUp.length + app + " a custom email action";
        } else if (apps.length > 0 && notSetUp.length === 0) {
          app = apps.length === 1 ? " App has " : " Apps have ";
          result = apps.length + app + " a custom email action";
        } else {
          result = "No Apps Created";
        }
      } else {
        /* The following logic is applied if the user if not a super user and an app is
         * selected OR if the user is a super user and is reviewing one of their own apps.
         * This then gives the specific status for the app */

        if (
          !this.$data.selectedApp.disableCustomUrls &&
          this.$data.selectedApp.customEmailConfirmationAction !== ""
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
        if (this.$data.user.isSuperUser) {
          apps = this.getApps;
        } else {
          apps = this.getUsersApps;
        }
      } else {
        apps.push(this.$data.selectedApp);
      }

      let notSetUp = [];

      if (apps.length > 0) {
        notSetUp = apps.filter(
          (app) =>
            app.disableCustomUrls ||
            (!app.disableCustomUrls && app.customEmailConfirmationAction === "")
        );
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
