<template>
  <v-card>
    <v-card-text>
      <v-container fluid>
        <v-card-title
          class="justify-center"
          v-if="
            app.isActive === false ||
            (app.devUrl === '' && app.inDevelopment) ||
            (app.liveUrl === '' && !app.inDevelopment)
          "
          >{{ app.name }}</v-card-title
        >
        <v-card-title
          class="justify-center"
          v-if="
            app.isActive &&
            ((app.devUrl !== '' && app.inDevelopment) ||
              (app.liveUrl !== '' && !app.inDevelopment))
          "
        >
          <a
            :href="app.inDevelopment ? app.devUrl : app.liveUrl"
            target="blank"
            class="app-card-title"
          >
            {{ app.name }}
          </a>
        </v-card-title>
        <hr class="title-spacer" />
        <v-card-title>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            label="Search"
            single-line
            hide-details
          ></v-text-field>
        </v-card-title>
        <v-data-table
          v-model="selectedUsers"
          :headers="headers"
          :items="app.users"
          show-select
          class="elevation-1"
          :search="search"
        >
        </v-data-table>
      </v-container>
    </v-card-text>
    <hr />
    <v-card-title class="justify-center">Available Actions</v-card-title>
    <v-card-actions>
      <v-container>
        <v-row dense>
          <v-col>
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="blue darken-1"
                  text
                  @click="refreshApp"
                  v-bind="attrs"
                  v-on="on"
                >
                  Refresh App
                </v-btn>
              </template>
              <span>Get your latest app data from the api</span>
            </v-tooltip>
          </v-col>
          <v-col v-if="filterNonAdmins">
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="blue darken-1"
                  text
                  @click="promoteToAdmins"
                  v-bind="attrs"
                  v-on="on"
                >
                  Promote to Admins
                </v-btn>
              </template>
              <span
                >Provide the selected users with admin rights to this app</span
              >
            </v-tooltip>
          </v-col>
          <v-col v-if="filterAdmins">
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="blue darken-1"
                  text
                  @click="demoteAdmins"
                  v-bind="attrs"
                  v-on="on"
                >
                  Demote Admins
                </v-btn>
              </template>
              <span>Demote the selected users admin rights to this app</span>
            </v-tooltip>
          </v-col>
          <v-col v-if="selectedUsers.length > 0">
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="red darken-1"
                  text
                  @click="removeUsers"
                  v-bind="attrs"
                  v-on="on"
                >
                  Remove Users
                </v-btn>
              </template>
              <span>Remove users from this app</span>
            </v-tooltip>
          </v-col>
        </v-row>
      </v-container>
    </v-card-actions>
  </v-card>
</template>

<script>
/* eslint-disable no-unused-vars */
import _ from "lodash";
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { appService } from "@/services/appService/app.service";
import App from "@/models/app";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";
import { convertStringToDateTime } from "@/helpers/commonFunctions/commonFunctions";

export default {
  name: "AppUsersWidget",
  data: () => ({
    app: new App(),
    search: "",
    selectedUsers: [],
    headers: [
      {
        text: "App Users",
        align: "start",
        sortable: true,
        value: "userName",
      },
      { text: "Id", value: "id" },
      { text: "First Name", value: "firstName" },
      { text: "Last Name", value: "lastName" },
      { text: "Admin", value: "isAdmin" },
      { text: "Signed Up Date", value: "signedUpDate" },
    ],
  }),
  methods: {
    ...mapActions("appModule", ["updateSelectedApp", "replaceApp"]),

    async refreshApp() {
      var response = await appService.getApp(this.$data.app.id);

      if (response.data.success) {
        this.$data.app = new App(response.data.app);
        const licenseResponse = await appService.getLicense(this.$data.app.id);
        if (licenseResponse.data.success) {
          this.$data.app.updateLicense(licenseResponse.data.license);
        }
        this.updateSelectedApp(this.$data.app);
        this.replaceApp(this.$data.app);
      }

      this.$data.selectedUsers = [];
    },

    async promoteToAdmins() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              let users = [];

              this.$data.selectedUsers.forEach((user) => {
                if (user.isAdmin === "No") {
                  user.isAdmin = false;
                } else {
                  user.isAdmin = true;
                }

                if (!user.isAdmin && user.id !== 1) {
                  users.push(new User(user));
                }

                if (user.isAdmin === false) {
                  user.isAdmin = "No";
                } else {
                  user.isAdmin = "Yes";
                }
              });

              let successes = 0;
              let errors = 0;
              let errorMessages = "";

              for (const user of users) {
                const response = await appService.putActivateAdminPrivileges(
                  this.$data.app.id,
                  user.id
                );

                if (response.status === 200) {
                  successes++;
                } else {
                  errors++;
                  errorMessages = response.data.message.substring(17);
                }
              }

              if (successes > 0 && errors === 0) {
                showToast(
                  this,
                  ToastMethods["success"],
                  `Users have been promoted to admins for ${this.$data.app.name}`,
                  defaultToastOptions()
                );
              } else if (successes > 0 && errors > 0) {
                showToast(
                  this,
                  ToastMethods["error"],
                  `Some users were not promoted with the following message(s): ${errorMessages}`,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  `Request failed with the following message(s): ${errorMessages}`,
                  defaultToastOptions()
                );
              }
            } catch (error) {
              showToast(
                this,
                ToastMethods["error"],
                error,
                defaultToastOptions()
              );
            }

            this.refreshApp();
          },
        },
        {
          text: "No",
          onClick: (e, toastObject) => {
            toastObject.goAway(0);
          },
        },
      ];

      showToast(
        this,
        ToastMethods["show"],
        `Are you sure you want to promote these users to admins for ${this.$data.app.name}?`,
        actionToastOptions(action, "mode_edit")
      );
    },

    async demoteAdmins() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              let users = [];

              this.$data.selectedUsers.forEach((user) => {
                if (user.isAdmin === "No") {
                  user.isAdmin = false;
                } else {
                  user.isAdmin = true;
                }

                if (user.isAdmin && user.id !== 1) {
                  users.push(new User(user));
                }

                if (user.isAdmin === false) {
                  user.isAdmin = "No";
                } else {
                  user.isAdmin = "Yes";
                }
              });

              let successes = 0;
              let errors = 0;
              let errorMessages = "";

              for (const user of users) {
                const response = await appService.putDeactivateAdminPrivileges(
                  this.$data.app.id,
                  user.id
                );

                if (response.status === 200) {
                  successes++;
                } else {
                  errors++;
                  errorMessages = response.data.message.substring(17);
                }
              }

              if (successes > 0 && errors === 0) {
                showToast(
                  this,
                  ToastMethods["success"],
                  `Users have been demoted from admins for ${this.$data.app.name}`,
                  defaultToastOptions()
                );
              } else if (successes > 0 && errors > 0) {
                showToast(
                  this,
                  ToastMethods["error"],
                  `Some users were not demoted with the following message(s): ${errorMessages}`,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  `Request failed with the following message(s): ${errorMessages}`,
                  defaultToastOptions()
                );
              }
            } catch (error) {
              showToast(
                this,
                ToastMethods["error"],
                error,
                defaultToastOptions()
              );
            }

            this.refreshApp();
          },
        },
        {
          text: "No",
          onClick: (e, toastObject) => {
            toastObject.goAway(0);
          },
        },
      ];

      showToast(
        this,
        ToastMethods["show"],
        `Are you sure you want to demote these admin users for ${this.$data.app.name}?`,
        actionToastOptions(action, "mode_edit")
      );
    },

    async removeUsers() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              let successes = 0;
              let errors = 0;
              let errorMessages = "";

              for (const user of this.$data.selectedUsers) {
                const response = await appService.deleteRemoveUser(
                  this.$data.app.id,
                  user.id
                );

                if (response.status === 200) {
                  successes++;
                } else {
                  errors++;
                  errorMessages = response.data.message.substring(17);
                }
              }

              if (successes > 0 && errors === 0) {
                showToast(
                  this,
                  ToastMethods["success"],
                  `Users have been demoted from admins for ${this.$data.app.name}`,
                  defaultToastOptions()
                );
              } else if (successes > 0 && errors > 0) {
                showToast(
                  this,
                  ToastMethods["error"],
                  `Some users were not demoted with the following message(s): ${errorMessages}`,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  `Request failed with the following message(s): ${errorMessages}`,
                  defaultToastOptions()
                );
              }
            } catch (error) {
              showToast(
                this,
                ToastMethods["error"],
                error,
                defaultToastOptions()
              );
            }

            this.refreshApp();
          },
        },
        {
          text: "No",
          onClick: (e, toastObject) => {
            toastObject.goAway(0);
          },
        },
      ];

      showToast(
        this,
        ToastMethods["show"],
        `Are you sure you want to remove this user from ${this.$data.app.name}?`,
        actionToastOptions(action, "delete")
      );
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getRequestorId"]),
    ...mapGetters("appModule", ["getSelectedApp"]),

    filterNonAdmins() {
      const filteredArray = _.filter(this.$data.selectedUsers, function (user) {
        return user.isAdmin === "No" && user.id !== 1;
      });
      return filteredArray.length > 0;
    },

    filterAdmins() {
      const filteredArray = _.filter(this.$data.selectedUsers, function (user) {
        return user.isAdmin === "Yes" && user.id !== 1;
      });
      return filteredArray.length > 0;
    },
  },
  watch: {
    "$store.state.appModule.selectedApp": {
      handler: function (val, oldVal) {
        this.$data.app = new App(this.getSelectedApp);

        this.$data.app.users.forEach((user) => {
          if (user.isAdmin === true) {
            user.isAdmin = "Yes";
          } else {
            user.isAdmin = "No";
          }

          user["signedUpDate"] = convertStringToDateTime(user.dateCreated);
        });
      },
    },
  },
  created() {
    this.$data.app = new App(this.getSelectedApp);

    this.$data.app.users.forEach((user) => {
      if (user.isAdmin === true) {
        user.isAdmin = "Yes";
      } else {
        user.isAdmin = "No";
      }

      user["signedUpDate"] = convertStringToDateTime(user.dateCreated);
    });
  },
};
</script>