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
  </v-card>
</template>

<script>
/* eslint-disable no-unused-vars */
import { mapGetters } from "vuex";
import { appService } from "@/services/appService/app.service";
import App from "@/models/app";
import User from "@/models/user";
import { convertStringToDateTime } from "@/helpers/commonFunctions/commonFunctions";

export default {
  name: "NonAppUsersWidget",
  data: () => ({
    app: new App(),
    nonAppUsers: [],
    search: '',
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
      { text: "Signed Up Date", value: "signedUpDate" },
    ],
  }),
  computed: {
    ...mapGetters("appModule", ["getSelectedApp"]),
  },
  watch: {
    "$store.state.appModule.selectedApp": {
      handler: async function(val, oldVal) {
        this.$data.app = new App(this.getSelectedApp);
        this.nonAppUsers = [];

        const response = await appService.getNonAppUsers(this.$data.app.id);

        if (response.data.success) {
          response.data.users.forEach((user) => {
            this.nonAppUsers.push(new User(user));
          });        
        }
        
        if (this.nonAppUsers.length > 0) {
          this.nonAppUsers.forEach((user) => {
            user["signedUpDate"] = convertStringToDateTime(user.dateCreated);
          });
        }
      }
    },
  },
  async created() {
    this.$data.app = new App(this.getSelectedApp);
    this.nonAppUsers = [];

    const response = await appService.getNonAppUsers(this.$data.app.id);

    if (response.data.success) {
      response.data.users.forEach((user) => {
        this.nonAppUsers.push(new User(user));
      });        
    }
    
    if (this.nonAppUsers.length > 0) {
      this.nonAppUsers.forEach((user) => {
        user["signedUpDate"] = convertStringToDateTime(user.dateCreated);
      });
    }
  },
}
</script>