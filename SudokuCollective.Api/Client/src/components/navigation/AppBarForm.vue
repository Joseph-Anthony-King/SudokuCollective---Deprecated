<template>
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

      <v-menu left bottom>
        <template v-slot:activator="{ on, attrs }">
          <v-btn v-bind="attrs" v-on="on" text>
            <span class="mr-2">Menu</span>
            <v-icon>mdi-dots-vertical</v-icon>
          </v-btn>
        </template>
        <v-list>
          <!-- User Functions -->
          <v-list-item v-if="!userLoggedIn">
            <v-list-item-content>
              <v-list-item-title>
                <div class="menu-item" @click="login">
                  <v-icon>mdi-login-variant</v-icon>
                  <span class="mr-2">Login</span>
                </div>
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item v-if="!userLoggedIn">
            <v-list-item-content>
              <v-list-item-title>
                <div class="menu-item" @click="signup">
                  <v-icon>mdi-account-plus</v-icon>
                  <span class="mr-2">Sign Up</span>
                </div>
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item v-if="userLoggedIn">
            <v-list-item-content>
              <v-list-item-title>
                <div class="menu-item" @click="logout">
                  <v-icon>mdi-logout-variant</v-icon>
                  <span class="mr-2">Logout</span>
                </div>
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <!-- outside links -->
          <hr v-if="appMenuItems.length > 1" class="mx-2" />
          <v-list-item v-if="appMenuItems.length > 1">
            <v-menu left bottom>
              <template v-slot:activator="{ on, attrs }">
                <div class="menu-item" v-bind="attrs" v-on="on">
                  <span class="mr-2">Links</span>
                </div>
              </template>
              <v-list class="menu-link-list">
                <!-- outside links -->
                <v-list-item
                  v-for="(menuItem, index) in appMenuItems"
                  :key="index"
                >
                  <v-list-item-content>
                    <v-list-item-title>
                      <a
                        :href="menuItem.url"
                        target="blank"
                        :title="menuItem.title"
                        class="menu-item"
                      >
                        <v-icon>{{ menuItem.mdiIcon }}</v-icon>
                        <span class="mr-2">{{ menuItem.title }}</span>
                      </a>
                    </v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </v-menu>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>    
</template>

<style scoped>
.menu-item {
  text-decoration: none !important;
  color: #9b9b9b;
  cursor: pointer;
}
.menu-link-list {
  width: 215px;
}
</style>

<script>
import { AppMenuItems } from "@/models/arrays/appMenuItems";

export default {
  name: "AppBarForm",
  props: ["userLoggedIn"],
  data: () => ({
    appMenuItems: [],
  }),
  methods: {

    login() {
      this.$emit("user-logging-in", null, null);
    },

    logout() {
      this.$emit("user-logging-out", null, null);
    },
    
    signup() {
      this.$emit("user-signing-up", null, null);
    },

    populateAppMenuItems() {
      AppMenuItems.forEach((route) => {
        this.$data.appMenuItems.push(route);
      });
    },
  },

  async created() {

    this.populateAppMenuItems();
  },

  mounted() {
  },
}
</script>