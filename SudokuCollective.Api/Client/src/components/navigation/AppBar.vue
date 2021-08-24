<template>
  <v-app-bar app color="primary" dark>
    <v-app-bar-nav-icon
      class="nav-bar-icon-status"
      @click.stop="updateNavDrawerStatus"
      v-if="userLoggedIn"
    ></v-app-bar-nav-icon>
    <div class="d-flex align-center">
      <router-link to="/">
        <v-img
          alt="Vuetify Logo"
          class="shrink mr-2"
          contain
          src="/images/logo.png"
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
          src="/images/name-logo.png"
          width="269"
        />
      </router-link>
      <div class="hidden-md-and-up name-logo">
        <router-link to="/">
          <v-img
            alt="Vuetify Name"
            contain
            max-width="180"
            src="/images/name-logo.png"
            transition="scale-transition"
            width="180"
          />
        </router-link>
      </div>
    </div>

    <v-spacer></v-spacer>

    <v-menu left bottom>
      <template v-slot:activator="{ on, attrs }">
        <v-btn v-bind="attrs" v-on="on" text>
          <span class="mr-2">Menu</span>
          <v-icon>mdi-dots-vertical</v-icon>
        </v-btn>
      </template>
      <v-list class="menu-list">
        <!-- User Functions -->
        <v-list-item v-if="!userLoggedIn">
          <v-list-item-content>
            <v-list-item-title>
              <router-link :to="solveNavigation.url" class="menu-item">
                <v-icon>{{ solveNavigation.icon }}</v-icon>
                {{ solveNavigation.title }}
              </router-link>
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item>
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
              <router-link :to="profileNavigation.url" class="menu-item">
                <v-icon>{{ profileNavigation.icon }}</v-icon>
                {{ profileNavigation.title }}
              </router-link>
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
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-list-item
              v-if="appMenuItems.length > 1"
              v-bind="attrs"
              v-on="on"
            >
              <v-menu left bottom>
                <template v-slot:activator="{ on, attrs }">
                  <div class="menu-item" v-bind="attrs" v-on="on">
                    <span class="mr-2">Links</span>
                  </div>
                </template>
                <v-list class="menu-link-list">
                  <!-- outside links -->
                  <div v-for="(menuItem, index) in appMenuItems" :key="index">
                    <v-tooltip bottom>
                      <template v-slot:activator="{ on, attrs }">
                        <v-list-item v-bind="attrs" v-on="on">
                          <v-list-item-content>
                            <v-list-item-title>
                              <a
                                :href="menuItem.url"
                                target="blank"
                                class="menu-item"
                              >
                                <v-icon>{{ menuItem.mdiIcon }}</v-icon>
                                <span class="mr-2">{{ menuItem.title }}</span>
                              </a>
                            </v-list-item-title>
                          </v-list-item-content>
                        </v-list-item>
                      </template>
                      <span>{{ menuItem.tooltip }}</span>
                    </v-tooltip>
                  </div>
                </v-list>
              </v-menu>
            </v-list-item>
          </template>
          <span>Links to outside sites and resources</span>
        </v-tooltip>
      </v-list>
    </v-menu>
  </v-app-bar>
</template>

<style scoped>
@media only screen and (min-width: 1367px) {
  .nav-bar-icon-status {
    display: none;
  }
}
.name-logo {
  display: flex;
  justify-content: center;
  align-items: center;
}
.menu-list {
  width: 220px;
}
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
  name: "AppBar",
  props: ["userLoggedIn", "profileNavigation", "solveNavigation"],
  data: () => ({
    appMenuItems: [],
  }),
  methods: {
    solve() {
      this.$emit("user-solving-sudoku", null, null);
    },

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

    updateNavDrawerStatus() {
      this.$emit("update-nav-drawer-status", null, null);
    },
  },

  async mounted() {
    this.populateAppMenuItems();
  },
};
</script>
