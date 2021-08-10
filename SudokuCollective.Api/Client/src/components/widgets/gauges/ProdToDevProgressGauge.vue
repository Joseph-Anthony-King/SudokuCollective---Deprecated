<template>
  <div class="text-center">
    <v-progress-circular
      :rotate="90"
      :size="310"
      :width="30"
      :value="value"
      color="success"
    >
      {{ value }}%
    </v-progress-circular>
    <h3>Development to Production</h3>
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
  name: "ProdToDevProgressGauge",
  data: () => ({
    user: new User(),
  }),
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getUsersApps", "getApps"]),

    value() {
      let apps;

      if (!this.$data.user.isSuperUser) {
        apps = this.getUsersApps;
      } else {
        apps = this.getApps;
      }

      let totalApps = apps.length;
      let totalDevelopment = _.filter(apps, function (app) {
        return app.inDevelopment;
      });
      let ratio = 0;

      if (apps.length > 0) {
        ratio = (totalDevelopment.length / totalApps) * 100;
      }

      return ratio.toFixed(0);
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

      if (apps.length > 0 && totalDevelopment.length > 0) {
        return (
          "Apps in Development: " +
          totalDevelopment.length +
          " Apps in Production: " +
          (apps.length - totalDevelopment.length)
        );
      } else if (apps.length > 0 && totalDevelopment.length === 0) {
        const app = apps.length === 1 ? "App is " : "All Apps are ";
        return app + "in Production";
      } else {
        return "No Apps Created";
      }
    },
  },
  created() {
    this.$data.user = this.getUser;
  },
};
</script>
