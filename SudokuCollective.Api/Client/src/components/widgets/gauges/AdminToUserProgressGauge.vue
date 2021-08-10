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
    <h3>Admins to Users</h3>
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
  name: "AdminToUserProgressGauge",
  data: () => ({
    user: new User(),
  }),
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getUsersApps"]),
    ...mapGetters("userModule", ["getUsers"]),

    value() {
      let totalUsers = 0;
      let totalAdmins = 0;
      let ratio = 0;

      if (!this.$data.user.isSuperUser) {
        const apps = this.getUsersApps;

        if (apps.length > 0) {
          apps.forEach((app) => {
            totalUsers += app.users.length;
            const admins = _.filter(app.users, function (user) {
              return user.isAdmin;
            });
            totalAdmins += admins.length;
          });

          ratio = (totalAdmins / totalUsers) * 100;
        }
      } else {
        const users = this.getUsers;
        totalUsers = users.length;

        users.forEach((user) => {
          if (user.isAdmin) {
            totalAdmins++;
          }
        });

        ratio = (totalAdmins / totalUsers) * 100;
      }

      return ratio.toFixed(0);
    },

    legend() {
      let apps = [];
      let totalUsers = 0;
      let totalAdmins = 0;

      if (!this.$data.user.isSuperUser) {
        apps = this.getUsersApps;

        if (apps.length > 0) {
          apps.forEach((app) => {
            totalUsers += app.users.length;
            const admins = _.filter(app.users, function (user) {
              return user.isAdmin;
            });
            totalAdmins += admins.length;
          });
        }
      } else {
        const users = this.getUsers;
        totalUsers = users.length;
        users.forEach((user) => {
          if (user.isAdmin) {
            totalAdmins++;
          }
        });
      }

      if (
        (!this.$data.user.isSuperUser && apps.length > 0) ||
        this.$data.user.isSuperUser
      ) {
        return "Admins: " + totalAdmins + " Users: " + totalUsers;
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
