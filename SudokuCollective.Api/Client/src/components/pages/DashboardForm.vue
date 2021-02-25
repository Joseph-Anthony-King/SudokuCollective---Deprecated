<template>
  <v-container fluid>
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title v-if="user.isLoggedIn" class="justify-center"
            >Dashboard</v-card-title
          >
          <v-card-title v-if="!user.isLoggedIn" class="justify-center warning"
            >Please Log In</v-card-title
          >
          <hr class="title-spacer" />
          <v-row>
            <v-card-text>Info will go here...</v-card-text>
          </v-row>
        </v-container>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script>
import User from "@/models/user";
import { mapGetters } from "vuex";

export default {
  name: "DashboardForm",

  data: () => ({
    user: {},
  }),

  methods: {},

  computed: {
    ...mapGetters("userModule", ["getUser"]),
  },

  created() {
    this.$data.apiMsg = this.getAPIMessage;
  },

  mounted() {
    this.$data.user = new User();
    this.$data.user = this.$store.getters["userModule/getUser"];
    this.$data.user.clone(this.getUser);
  },

  updated() {
  },
};
</script>
