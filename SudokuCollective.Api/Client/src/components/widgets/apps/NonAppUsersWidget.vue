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
          :items="nonAppUsers"
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
          <v-col v-if="selectedUsers.length > 0">
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="blue darken-1"
                  text
                  @click="addUsers"
                  v-bind="attrs"
                  v-on="on"
                >
                  Add Users
                </v-btn>
              </template>
              <span>Add registered users to your app</span>
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
  name: "NonAppUsersWidget",
  data: () => ({
    app: new App(),
    nonAppUsers: [],
    search: "",
    selectedUsers: [],
    headers: [
      {
        text: "Non-App Users",
        align: "start",
        sortable: true,
        value: "userName",
      },
      { text: "Id", value: "id" },
      { text: "First Name", value: "firstName" },
      { text: "Last Name", value: "lastName" },
      { text: "Signed Up Date", value: "dateCreated" },
    ],
  }),
  methods: {
    ...mapActions("appModule", ["updateUsersSelectedApp", "replaceUsersApp"]),

    async refreshApp() {
      this.$data.app = this.getUsersSelectedApp;

      const response = await appProvider.getNonAppUsers(this.$data.app.id);

      this.$data.nonAppUsers = [];

      if (response.success) {
        response.users.forEach((user) => {
          this.$data.nonAppUsers.push(new User(user));
        });
      }
    },

    async addUsers() {
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
                const response = await appProvider.addUser(
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
                  `Users have been added to ${this.$data.app.name}`,
                  defaultToastOptions()
                );
              } else if (successes > 0 && errors > 0) {
                showToast(
                  this,
                  ToastMethods["error"],
                  `Some users were not added with the following message(s): ${errorMessages}`,
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

            var response = await appProvider.getApp(this.$data.app.id);

            if (response.success) {
              this.$data.app = response.app;
              this.updateUsersSelectedApp(this.$data.app);
              this.replaceUsersApp(this.$data.app);
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
        `Are you sure you want to add this users to ${this.$data.app.name}?`,
        actionToastOptions(action, "mode_edit")
      );
    },
  },
  computed: {
    ...mapGetters("appModule", ["getUsersSelectedApp"]),
  },
  watch: {
    "$store.state.appModule.usersSelectedApp": {
      handler: function (val, oldVal) {
        this.refreshApp();
      },
    },
  },
  created() {
    this.refreshApp();
  },
};
</script>
