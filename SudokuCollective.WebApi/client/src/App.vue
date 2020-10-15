<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <div class="d-flex align-center">
        <v-img
          alt="Vuetify Logo"
          class="shrink mr-2"
          contain
          src="/logo.png"
          transition="scale-transition"
          width="40"
        />

        <v-img
          alt="Vuetify Name"
          class="shrink mt-1 hidden-sm-and-down"
          contain
          min-width="269"
          src="/name-logo.png"
          width="269"
        />
      </div>
      <v-spacer></v-spacer>

      <v-btn
        href="https://github.com/vuetifyjs/vuetify/releases/latest"
        target="_blank"
        text
      >
        <span class="mr-2">Latest Release</span>
        <v-icon>mdi-open-in-new</v-icon>
      </v-btn>
    </v-app-bar>

    <v-main>
      <v-row class="text-center">
          <v-col cols="12">
              <router-link to="/">Home</router-link> |
              <a :href=apiUrl target="blank">API Status Page</a> |
              <a href=https://github.com/Joseph-Anthony-King/SudokuCollective target="blank">View Code on GitHub</a> |
              <router-link to="/login">Login</router-link>
          </v-col>
      </v-row>
      <router-view />
    </v-main>
  </v-app>
</template>

<script>
    import { mapActions } from "vuex";

    export default {

        name: "App",

        data: () => ({
            apiUrl: ""
        }),

        methods: {
            ...mapActions("appSettingsModule", ["confirmBaseURL"])
        },

        async created() {

            await this.confirmBaseURL();

            this.apiUrl = this.$store.getters["appSettingsModule/getApiURL"];
        },
    };
</script>
