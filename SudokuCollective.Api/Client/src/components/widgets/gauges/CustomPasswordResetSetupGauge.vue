<template>
  <div class="text-center">
    <v-progress-circular
      :rotate="90"
      :size="310"
      :width="30"
      :value="value"
      :color="isApplicable ? 'error' : 'success'"
    >
      {{ value }}%
    </v-progress-circular>
    <h3>Password Reset Page Setup</h3>
    <h3>{{ legend }}</h3>
  </div>
</template>

<style scoped>
.v-progress-circular {
  margin: 2rem;
}
</style>

<script>
import _ from "lodash";
import { mapGetters } from "vuex";
import User from "@/models/user";

export default {
  name: "CustomPasswordResetSetupGauge",
  data: () => ({
    user: new User(),
  }),
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getUsersApps", "getApps"]),

    value() {
      let apps = [];

      if (!this.$data.user.isSuperUser) {
        apps = this.getUsersApps;
      } else {
        apps = this.getApps;
      }

      let setUp;
      let ratio = 0;

      if (apps.length > 0) {
        setUp = _.filter(apps, function (app) {
          return app.disableCustomUrls && app.customPasswordResetAction === "";
        });

        ratio = (setUp.length / apps.length) * 100;
      }

      return ratio.toFixed(0);
    },

    legend() {
      let apps = [];

      if (!this.$data.user.isSuperUser) {
        apps = this.getUsersApps;
      } else {
        apps = this.getApps;
      }

      let setUp;

      if (apps.length > 0) {
        setUp = _.filter(apps, function (app) {
          return app.disableCustomUrls && app.customPasswordResetAction === "";
        });
      }

      let app;

      if (apps.length > 0 && setUp.length > 0) {
        app = setUp.length === 1 ? " App needs " : " Apps need ";
        return setUp.length + app + " a password reset action";
      } else if (apps.length > 0 && setUp.length == 0) {
        app = apps.length === 1 ? " App " : " Apps ";
        return apps.length + app + " have a password action";
      } else {
        return "No Apps Created";
      }
    },

    isApplicable() {
      const apps = this.getUsersApps;

      let setUp = [];

      if (apps.length > 0) {
        setUp = _.filter(apps, function (app) {
          return (
            app.disableCustomUrls ||
            (!app.disableCustomUrls && app.customPasswordResetAction === "")
          );
        });
      }

      if (setUp.length > 0) {
        return true;
      } else {
        return false;
      }
    },
  },
  created() {
    this.$data.user = this.getUser;
  },
};
</script>
