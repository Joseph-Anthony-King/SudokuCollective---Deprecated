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
            :single-select="singleSelect"
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
    <ReviewUserWidget
      v-if="selectedUsers.length > 0"
      v-on:close-review-user-widget-event="closeUserWidget"
    />
  </div>
</template>

<style scoped></style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import ReviewUserWidget from "@/components/widgets/users/ReviewUserWidget";
import User from "@/models/user";

export default {
  name: "UsersWidget",
  components: {
    ReviewUserWidget,
  },
  data: () => ({
    users: [],
    search: "",
    selectedUsers: [],
    singleSelect: true,
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
      { text: "Email Confirmed", value: "emailConfirmed" },
      { text: "Admin", value: "admin" },
      { text: "Active", value: "active" },
      { text: "Licenses", value: "licenses" },
      { text: "Signed Up Date", value: "dateCreated" },
    ],
    processing: false,
  }),
  methods: {
    ...mapActions("userModule", ["updateSelectedUser"]),
    closeUserWidget() {
      this.$data.selectedUsers = [];
    },
  },
  computed: {
    ...mapGetters("userModule", ["getUsers"]),

    title() {
      const users = this.$data.users.length == 1 ? "User" : "Users";
      return this.$data.users.length + " " + users + " Currently Registered";
    },
  },
  watch: {
    selectedUsers: {
      handler: function (val, oldVal) {
        if (val.length > 0) {
          this.updateSelectedUser(val[0]);
        } else {
          this.updateSelectedUser(new User());
        }
      },
    },
  },
  async created() {
    this.$data.processing = true;
    this.$data.users = this.getUsers;
    this.$data.processing = false;
  },
};
</script>
