<template>
  <v-navigation-drawer
    app
    color="secondary"
    :value="navDrawerStatus"
    v-if="userLoggedIn"
    mobile-breakpoint="1367"
  >
    <v-list>
      <v-list-item>
        <v-list-item-content>
          <router-link :to="profileNavigation.url" class="user-profile-item">
            <v-icon x-large color="white">{{ profileNavigation.icon }}</v-icon>
          </router-link>
        </v-list-item-content>
      </v-list-item>
      <v-container class="navigation-status-indicator">
        <span class="user-profile-subscript">{{ greeting }}</span>
      </v-container>
      <v-container class="navigation-status-indicator">
        <router-link :to="profileNavigation.url" class="nav-drawer-item">
          <span class="user-profile-item">{{ user.userName }}</span>
        </router-link>
      </v-container>
    </v-list>
    <v-list v-if="navMenuItems.length > 1">
      <v-list-item v-for="(navItem, index) in navMenuItems" :key="index">
        <v-list-item-content>
          <v-list-item-title>
            <v-icon class="white--text">{{ navItem.mdiIcon }}</v-icon>
            <router-link :to="navItem.url" class="nav-drawer-item">{{
              navItem.title
            }}</router-link>
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<style scoped>
.nav-drawer-item {
  font-weight: bold;
  text-decoration: none !important;
  color: #ffffff;
}
.user-profile-item {
  font-weight: bold;
  text-decoration: none !important;
  text-transform: uppercase;
  text-align: center;
  color: #ffffff;
}
.user-profile-subscript {
  font-size: small;
  text-align: center;
  color: #ffffff;
}
.navigation-status-indicator {
  text-align: center;
  padding: 0;
  margin: 0;
}

.white--text {
  padding-right: 10px;
  padding-bottom: 5px;
}
</style>

<script>
/* eslint-disable no-unused-vars */
import _ from "lodash";
import { mapGetters } from "vuex";
import User from "@/models/user";
import MenuItem from "@/models/viewModels/menuItem";

export default {
  name: "NavigationBar",
  props: ["userLoggedIn", "navDrawerStatus", "profileNavigation"],
  data: () => ({
    navMenuItems: [],
    user: {},
    greeting: "",
  }),
  methods: {
    updateGreeting: function () {
      this.updateNow();

      setInterval(() => {
        this.updateNow();
      }, 60000);
    },

    populateNavMenuItems() {
      this.$router.options.routes.forEach((route) => {
        const routeNames = ["UserProfile", "ConfirmEmail", "ResetPassword"];

        if (!_.includes(routeNames, route.name)) {
          let icon;

          if (route.name === "Home") {
            icon = "mdi-home";
          } else if (route.name === "Dashboard") {
            icon = "mdi-view-dashboard";
          } else {
            icon = "mdi-puzzle";
          }

          this.$data.navMenuItems.push(
            new MenuItem(route.path, route.name, icon)
          );
        }
      });
    },

    updateNow() {
      const now = new Date();

      if (now.getHours() < 12) {
        this.$data.greeting = "Good Morning";
      } else if (now.getHours() < 18) {
        this.$data.greeting = "Good Afternoon";
      } else {
        this.$data.greeting = "Good Evening";
      }
    }
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
  },
  watch: {
    "$store.state.settingsModule.user": {
      handler: function (val, oldVal) {
        this.$data.user = new User(this.getUser);
      },
    },
  },
  async created() {
    this.updateGreeting();

    this.populateNavMenuItems();

    this.$data.user = new User(this.getUser);
  },
};
</script>
