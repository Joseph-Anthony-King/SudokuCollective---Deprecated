<template>
  <v-form>
    <v-container>
      <div v-if="!user.isLoggedIn">
        <v-row>
          <v-col cols="12">
            <h1>Please Login</h1>
          </v-col>
        </v-row>
      </div>
      <div v-if="user.isLoggedIn">
        <h1 class="display-2 font-weight-bold mb-3">
          {{ apiMsg }}
        </h1>
        <h2>{{ user.userName }} is logged in!</h2>
      </div>
    </v-container>
  </v-form>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  name: "DashboardForm",

  data: () => ({
    apiMsg: "",
    user: {},
  }),

  methods: {},

  computed: {
    ...mapGetters("appSettingsModule", ["getAPIMessage"]),
    ...mapGetters("userModule", ["getUser"]),
  },

  created() {
    this.$data.apiMsg = this.getAPIMessage;
  },

  mounted() {
    this.$data.user = this.$store.getters["userModule/getUser"];
    this.$data.user.clone(this.getUser);
  },

  updated() {
    this.$data.apiMsg = this.$store.getters["appSettingsModule/getAPIMessage"];
  },
};
</script>
