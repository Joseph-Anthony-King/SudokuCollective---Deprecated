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
        :headers="headers"
        :items="app.users"
        :items-per-page="5"
        class="elevation-1"
      >
      </v-data-table>
    </v-container>
  </v-card>
</template>

<script>
import App from "@/models/app";
import { mapGetters } from "vuex";

export default {
  name: "AppUsersWidget",
  data: () => ({
    app: new App(),
    headers: [
      {
        text: 'App Users',
        align: 'start',
        sortable: true,
        value: 'userName',
      },
      { text: 'Id', value: 'id' },
      { text: 'First Name', value: 'firstName' },
      { text: 'Last Name', value: 'lastName' },
      { text: 'Admin', value: 'isAdmin' },
    ],
  }),
  methods: {    

    isAdmin(user) {
      if (user.isAdmin) {
        return "Yes";
      } else {
        return "No"
      }
    }
  },
  computed: {    
    ...mapGetters("appModule", ["getSelectedApp"]),
  },
  watch: {
    "$store.state.appModule.selectedApp": function () {
      this.$data.app = new App(this.getSelectedApp);
    },
  },
  created() {
    this.$data.app = new App(this.getSelectedApp);
  },
}
</script>