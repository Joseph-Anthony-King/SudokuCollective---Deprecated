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
    users: [],
    selectedApp: new App(),
    processing: false
  }),
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getUsersSelectedApp", "getUsersApps"]),
    ...mapGetters("userModule", ["getUsers"]),

    value() {
      console.log("value invoked...");
      let totalUsers = 0;
      let totalAdmins = 0;
      let ratio = 0;

      if (!this.$data.user.isSuperUser) {
        // The following logic is applied if the user is not a super user
        if (this.$data.selectedApp.id === 0) {
          /* If no app is selected each app is reviewed and the admin to
           * to user statistics are pulled. Each admin/user is filtered to
           * prevent duplicates */
          const apps = this.getUsersApps;

          if (apps.length > 0) {
            let admins = [];
            let users = [];

            apps.forEach((app) => {
              const appAdmins = _.filter(app.users, function (user) {
                return user.isAdmin;
              });
              appAdmins.forEach((admin) => {
                if (!_.some(admins, admin)) {
                  admins.push(admin);
                }
              });
              const appUsers = _.filter(app.users, function (user) {
                return !user.isAdmin;
              });
              appUsers.forEach((admin) => {
                if (!_.some(admins, admin)) {
                  users.push(admin);
                }
              });
            });

            totalAdmins = admins.length;
            totalUsers = users.length;

            ratio = (totalAdmins / (totalAdmins + totalUsers)) * 100;
          }
        } else {
          // Users selected app user statistics
          this.$data.selectedApp.users.forEach((user) => {
            if (user.isAdmin) {
              totalAdmins++;
            }
          });
          
          totalUsers = this.$data.selectedApp.users.length;

          ratio = (totalAdmins / totalUsers) * 100;
        }
      } else {
        // The following logic is applied if the user is a super user
        if (this.$data.user.id !== this.$data.selectedApp.ownerId) {
          /* If the super user is not the owner of the selected 
           * app collective wide user statiscs are displayed */
          const collectiveUsers = this.getUsers;
          
          let admins = [];
          let users = [];

          collectiveUsers.forEach((user) => {
            if (user.isAdmin) {
              admins.push(user);
            } else {
              users.push(user);
            }
          });

          totalAdmins = admins.length;
          totalUsers = users.length;

          if (totalUsers !== 0 && totalAdmins !== 0) {
            ratio = (totalAdmins / (totalUsers + totalAdmins)) * 100;
          } else {
            ratio = 0;
          }
        } else {
          /* If the super user is the owner of the selected 
           * app the app's user statistics are displyed */
          this.$data.selectedApp.users.forEach((user) => {
            if (user.isAdmin) {
              totalAdmins++;
            }
          });
          
          totalUsers = this.$data.selectedApp.users.length;

          ratio = (totalAdmins / totalUsers) * 100;
        }
      }

      return ratio.toFixed(0) + "%";
    },

    legend() {
      let apps = [];
      let totalUsers = 0;
      let totalAdmins = 0;

      if (!this.$data.user.isSuperUser) {
        // The following logic is applied if the user is not a super user
        if (this.$data.selectedApp.id == 0) {
          /* If no app is selected each app is reviewed and the admin to
           * to user statistics are pulled. Each admin/user is filtered to
           * prevent duplicates */
          apps = this.getUsersApps;

          if (apps.length > 0) {
            let admins = [];
            let users = [];

            apps.forEach((app) => {
              const appAdmins = _.filter(app.users, function (user) {
                return user.isAdmin;
              });
              appAdmins.forEach((admin) => {
                if (!_.some(admins, admin)) {
                  admins.push(admin);
                }
              });
              const appUsers = _.filter(app.users, function (user) {
                return !user.isAdmin;
              });
              appUsers.forEach((admin) => {
                if (!_.some(admins, admin)) {
                  users.push(admin);
                }
              });
            });

            totalAdmins = admins.length;
            totalUsers = users.length;
          }
        } else {
          // Users selected app user statistics
          this.$data.selectedApp.users.forEach((user) => {
            if (user.isAdmin) {
              totalAdmins++;
            }
          });
          totalUsers = this.$data.selectedApp.users.length - totalAdmins;
        }
      } else {
        // The following logic is applied if the user is a super user
        let users = [];

        if (this.$data.user.id === this.$data.selectedApp.ownerId) {
          /* If the super user is the owner of the selected 
           * app the app's user statistics are displyed */
          this.$data.selectedApp.users.forEach((user) => {
            if (user.isAdmin) {
              totalAdmins++;
            }
          });
          
          totalUsers = this.$data.selectedApp.users.length - totalAdmins;
        } else {
          /* If the super user is not the owner of the selected 
           * app collective wide user statiscs are displayed */
          users = this.getUsers;

          users.forEach((user) => {
            if (user.isAdmin) {
              totalAdmins++;
            }
          });
          totalUsers = users.length - totalAdmins;
        }
      }

      if (this.$data.user.isSuperUser || apps.length > 0 || this.$data.selectedApp.id !== 0) {
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
    "value": {
      handler: function (val, oldVal) {
        console.log("value oldVal:", oldVal);
        console.log("value val:", val);
      }
    }
  },
  created() {
    this.$data.processing = true;
    this.$data.user = this.getUser;
    this.$data.processing = false;
  },
};
</script>
