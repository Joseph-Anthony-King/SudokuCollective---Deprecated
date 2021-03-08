<template>
  <v-card>
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
        :headers="adminHeaders"
        :items="app.users"
        show-select
        class="elevation-1"
        v-if="app.id === 1"
        :search="search"
        >
      </v-data-table>
      <v-data-table
        v-model="selectedUsers"
        :headers="headers"
        :items="app.users"
        show-select
        class="elevation-1"
        v-if="app.id !== 1"
        :search="search"
        >
      </v-data-table>
    </v-container>
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
                  Refresh List
                </v-btn>
              </template>
              <span>Get your latest app data from the api</span>
            </v-tooltip>
          </v-col>
          <v-col>
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="blue darken-1"
                  text
                  @click="promoteToAdmins"
                  :disabled="disableAdminButton"
                  v-bind="attrs"
                  v-on="on"
                >
                  Promote to Admins
                </v-btn>
              </template>
              <span>Provide the selected uses with admin rights to this app</span>
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
import PageListModel from "@/models/viewModels/pageListModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";
import { convertStringToDateTime } from "@/helpers/commonFunctions/commonFunctions"

export default {
  name: "AppUsersWidget",
  data: () => ({
    app: new App(),
    search: '',
    selectedUsers: [],
    adminHeaders: [
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
      { text: "Game Count", value: "gameCount"},
      { text: "Admin", value: "isAdmin" },
      { text: "Signed Up Date", value: "signedUpDate" },
    ],
  }),
  methods: {  
    ...mapActions("appModule", [
      "updateSelectedApp",
      "replaceApp"
    ]), 

    async refreshApp() {
      var response = await appService.getApp(this.$data.app.id, true);

      if (response.data.success) {
        this.$data.app = new App(response.data.app);
        const licenseResponse = await appService.getLicense(this.$data.app.id);
        if (licenseResponse.data.success) {
          this.$data.app.updateLicense(licenseResponse.data.license);
        }
        this.updateSelectedApp(this.$data.app);
        this.replaceApp(this.$data.app);
      }
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
                
                if (!user.isAdmin) {
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

                const data = {
                  license: this.$data.app.license,
                  requestorId: this.getRequestorId,
                  appId: this.$data.app.id,
                  pageListModel: new PageListModel()
                };

                const response = await appService.postObtainAdminPrivileges(
                  user.id,
                  this.$data.app.license
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
        "Are you sure you want to add admin privileges to these account?",
        actionToastOptions(action, "mode_edit")
      );
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getRequestorId"]),
    ...mapGetters("appModule", ["getSelectedApp"]),

    disableAdminButton() {
      // At least one selected user is not an admin
      return _.filter(this.$data.selectedUsers, function(user) { 
        user.isAdmin && user.id !== 1;
      }).length === 0;
    },
  },
  watch: {
    "$store.state.appModule.selectedApp": {
      handler: function(val, oldVal) {
        this.$data.app = new App(this.getSelectedApp);

        this.$data.app.users.forEach((user) => {
          if (user.isAdmin === true) {
            user.isAdmin = "Yes";
          } else {
            user.isAdmin = "No";
          }

          user["signedUpDate"] = convertStringToDateTime(user.dateCreated);
        });
      }
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
}
</script>