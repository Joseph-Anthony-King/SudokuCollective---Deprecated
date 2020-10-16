<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <div class="d-flex align-center">
        <router-link to="/">
          <v-img
            alt="Vuetify Logo"
            class="shrink mr-2"
            contain
            src="/logo.png"
            transition="scale-transition"
            width="40"
          />
        </router-link>

        <router-link to="/">
          <v-img
            alt="Vuetify Name"
            class="shrink mt-1 hidden-sm-and-down"
            contain
            min-width="269"
            src="/name-logo.png"
            width="269"
          />
        </router-link>
      </div>

      <v-spacer></v-spacer>

      <v-btn
        :href=apiUrl
        target="_blank"
        text
      >
        <span class="mr-2">API Status</span>
        <v-icon>mdi-open-in-new</v-icon>
      </v-btn>

      <v-btn
        :href=apiDocumentationUrl
        target="_blank"
        text
      >
        <span class="mr-2">API Documentation</span>
        <v-icon>mdi-open-in-new</v-icon>
      </v-btn>

      <v-btn
        href="https://github.com/Joseph-Anthony-King/SudokuCollective"
        target="_blank"
        text
      >
        <span class="mr-2">GitHub Page</span>
        <v-icon>mdi-open-in-new</v-icon>
      </v-btn>
    </v-app-bar>

    <v-content>
      
      <v-navigation-drawer
        absolute
        permanent
        color="secondary"
      >

        <v-list>
        
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>
                <router-link to="/">Home</router-link>
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>
                <router-link to="/login">Login</router-link>
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>

        </v-list>

      </v-navigation-drawer>

      <main>
        <transition name="fade">
            <router-view></router-view>
        </transition>
      </main>

    </v-content>

  </v-app>
</template>

<script>
    import { mapActions } from "vuex";

    export default {

        name: "App",

        data: () => ({
            apiUrl: "",
            apiDocumentationUrl: ""
        }),

        methods: {
            ...mapActions("appSettingsModule", ["confirmBaseURL"])
        },

        async created() {

            await this.confirmBaseURL();

            this.apiUrl = this.$store.getters["appSettingsModule/getApiURL"];
            this.apiDocumentationUrl = `${this.apiUrl}/swagger/index.html`
        },
    };
</script>
