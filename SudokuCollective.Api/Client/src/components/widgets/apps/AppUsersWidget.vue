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
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { appProvider } from "@/providers/appProvider";
import App from "@/models/app";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

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
      { text: "Admin", value: "admin" },
      { text: "Signed Up Date", value: "dateCreated" },
    ],
  }),
  methods: {
    ...mapActions("appModule", ["updateUsersSelectedApp", "replaceUsersApp"]),

    async refreshApp() {
      var response = await appProvider.getApp(this.$data.app.id);

      if (response.isSuccess) {
        this.$data.app = response.app;
        this.updateUsersSelectedApp(this.$data.app);
        this.replaceUsersApp(this.$data.app);
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
                if (!user.isAdmin && user.id !== 1) {
                  users.push(user);
                }
              });

              let successes = 0;
              let errors = 0;
              let errorMessages = "";

              for (const user of users) {
                const response = await appProvider.activateAdminPrivileges(
                  this.$data.app.id,
                  user.id
                );

                if (response.status === 200) {
                  successes++;
                } else {
                  errors++;
                  errorMessages = errorMessages + response.message + " ";
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
                if (user.isAdmin && user.id !== 1) {
                  users.push(new User(user));
                }
              });

              let successes = 0;
              let errors = 0;
              let errorMessages = "";

              for (const user of users) {
                const response = await appProvider.deactivateAdminPrivileges(
                  this.$data.app.id,
                  user.id
                );

                if (response.isSuccess) {
                  successes++;
                } else {
                  errors++;
                  errorMessages = errorMessages + response.message + " ";
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
                const response = await appProvider.removeUser(
                  this.$data.app.id,
                  user.id
                );

                if (response.status === 200) {
                  successes++;
                } else {
                  errors++;
                  errorMessages = errorMessages + response.message + " ";
                }
              }

              if (successes > 0 && errors === 0) {
                showToast(
                  this,
                  ToastMethods["success"],
                  `Users have been removed from ${this.$data.app.name}`,
                  defaultToastOptions()
                );
              } else if (successes > 0 && errors > 0) {
                showToast(
                  this,
                  ToastMethods["error"],
                  `Some users were not removed with the following message(s): ${errorMessages}`,
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

      var pronoun =
        this.$data.selectedUsers.length === 1 ? "this user" : "these users";

      showToast(
        this,
        ToastMethods["show"],
        `Are you sure you want to remove ${pronoun} from ${this.$data.app.name}?`,
        actionToastOptions(action, "delete")
      );
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getRequestorId"]),
    ...mapGetters("appModule", ["getUsersSelectedApp"]),

    filterNonAdmins() {
      const filteredArray = this.$data.selectedUsers.filter(
        (user) => user.admin === "No" && user.id !== 1
      );
      return filteredArray.length > 0;
    },

    filterAdmins() {
      const filteredArray = this.$data.selectedUsers.filter(
        (user) => user.admin === "Yes" && user.id !== 1
      );
      return filteredArray.length > 0;
    },
  },
  watch: {
    "$store.state.appModule.usersSelectedApp": {
      handler: function (val, oldVal) {
        this.$data.app = new App(this.getUsersSelectedApp);
      },
    },
  },
  mounted() {
    this.$data.app = new App(this.getUsersSelectedApp);
  },
};
</script>
