<template>
  <div class="text-center">
    <v-progress-circular
      :rotate="90"
      :size="310"
      :width="30"
      :value="value"
      color="success"
    >
      {{ value }}
    </v-progress-circular>
    <h3>Admins to Users</h3>
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
  name: "AdminToUserProgressGauge",
  data: () => ({
    user: new User(),
    selectedApp: new App(),
  }),
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getUsersSelectedApp", "getUsersApps"]),
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

      return ratio.toFixed(0) + "%";
    },

    legend() {
      let apps = [];
      let totalUsers = 0;
      let totalAdmins = 0;

      if (!this.$data.user.isSuperUser) {
        if (this.$data.selectedApp.id == 0) {
          apps = this.getUsersApps;

          if (apps.length > 0) {
            apps.forEach((app) => {
              const admins = _.filter(app.users, function (user) {
                return user.isAdmin;
              });
              totalAdmins += admins.length;
              totalUsers += app.users.length - admins.length;
            });
          }
        } else {
          this.$data.selectedApp.users.forEach((user) => {
            if (user.isAdmin) {
              totalAdmins++;
            }
          });
          totalUsers = this.$data.selectedApp.users.length - totalAdmins;
        }
      } else {
        const users = this.getUsers;

        users.forEach((user) => {
          if (user.isAdmin) {
            totalAdmins++;
          }
        });
        totalUsers = users.length - totalAdmins;
      }

      if (
        this.$data.user.isSuperUser ||
        (!this.$data.user.isSuperUser &&
          (apps.length > 0 || this.$data.selectedApp.id !== 0))
      ) {
        return "Admins: " + totalAdmins + " Users: " + totalUsers;
      } else {
        return "No Apps Created";
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
