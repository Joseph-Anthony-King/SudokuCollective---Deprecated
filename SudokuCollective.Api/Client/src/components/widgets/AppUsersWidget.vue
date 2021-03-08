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

    promoteToAdmins() {
      console.log(this.$data.selectedUsers);
    },
  },
  computed: {    
    ...mapGetters("appModule", ["getSelectedApp"]),

    disableAdminButton() {
      // At least one selected user is not an admin
      return _.filter(this.$data.selectedUsers, function(user) { 
        user.isAdmin;
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
    "selectedUsers": {
      handler: function(val, oldVal) {
        console.log("old value:", oldVal);
        console.log("new value:", val);
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