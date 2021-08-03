<template>
  <div v-if="!processing">
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">
            {{ title }}
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
            :items="users"
            show-select
            class="elevation-1"
            :search="search"
          >
          </v-data-table>
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
  </div>
</template>

<style scoped>

</style>

<script>
import { mapGetters } from "vuex";

export default {
  name: "UsersWidget",
  data: () => ({
    users: [],
    search: "",
    selectedUsers: [],
    headers: [
      {
        text: "Users",
        align: "start",
        sortable: true,
        value: "userName",
      },
      { text: "Id", value: "id" },
      { text: "First Name", value: "firstName" },
      { text: "Last Name", value: "lastName" },
      { text: "Email", value: "email" },
      { text: "Admin", value: "admin" },
      { text: "Licenses", value: "licenses" },
      { text: "Signed Up Date", value: "dateCreated" },
    ],
    processing: false
  }),
  computed: {
    ...mapGetters("userModule", ["getUsers"]),

    title() {
      const users = this.$data.users.length == 1 ? "User" : "Users"
      return this.$data.users.length + " " + users + " Currently Registered";
    }
  },
  async created() {
    this.$data.processing = true;
    this.$data.users = this.getUsers;
    this.$data.processing = false;
  }
}
</script>
