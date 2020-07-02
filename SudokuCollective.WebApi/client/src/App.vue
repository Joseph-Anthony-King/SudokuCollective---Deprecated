<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <div class="d-flex align-center">
        <v-img
          alt="Vuetify Logo"
          class="shrink mr-2"
          contain
          src="https://cdn.vuetifyjs.com/images/logos/vuetify-logo-dark.png"
          transition="scale-transition"
          width="40"
        />

        <v-img
          alt="Vuetify Name"
          class="shrink mt-1 hidden-sm-and-down"
          contain
          min-width="100"
          src="https://cdn.vuetifyjs.com/images/logos/vuetify-name-dark.png"
          width="100"
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

    <v-content>
      <v-row class="text-center">
        <v-col cols="12">
          <router-link to="/">Home</router-link> |
          <router-link to="/login">Login</router-link> | 
          <a :href=apiUrl target="blank">API</a>
        </v-col>
      </v-row>
      <router-view />
    </v-content>
  </v-app>
</template>

<script>

export default {
  name: "App",

  data: () => ({
    apiUrl: process.env.VUE_APP_API
  })
};
</script>
