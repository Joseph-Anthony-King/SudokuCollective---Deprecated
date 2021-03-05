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
      <v-data-table
        v-model="selectedUsers"
        :headers="adminHeaders"
        :items="app.users"
        show-select
        class="elevation-1"
        v-if="app.id === 1"
        >
      </v-data-table>
      <v-data-table
        v-model="selectedUsers"
        :headers="headers"
        :items="app.users"
        show-select
        class="elevation-1"
        v-if="app.id !== 1"
        >
      </v-data-table>
    </v-container>
  </v-card>
</template>

<script>
/* eslint-disable no-unused-vars */
import App from "@/models/app";
import { mapGetters } from "vuex";

export default {
  name: "AppUsersWidget",
  data: () => ({
    app: new App(),
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
    ],
  }),
  computed: {    
    ...mapGetters("appModule", ["getSelectedApp"]),
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
    });
  },
}
</script>