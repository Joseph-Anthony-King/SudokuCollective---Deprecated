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
            <v-icon class="white--text">{{navItem.mdiIcon}}</v-icon>
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
import { mapGetters } from "vuex";
import User from "@/models/user";
import MenuItem from "@/models/viewModels/menuItem";

export default {
  name: "NavigationBar",
  props: ["userLoggedIn", "navDrawerStatus", "profileNavigation"],
  data: () => ({
    navMenuItems: [],
    user: {},
  }),
  methods: {
    populateNavMenuItems() {
      this.$router.options.routes.forEach((route) => {
        if (route.name !== "UserProfile") {
          let icon;

          if (route.name === "Home") {
            icon = "mdi-home"
          } else if (route.name === "Dashboard") {
            icon = "mdi-view-dashboard"
          } else {
            icon = "mdi-apps"
          }

          this.$data.navMenuItems.push(
            new MenuItem(route.path, route.name, icon)
          );
        }
      });
      console.log(this.$data.navMenuItems);
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),

    greeting() {
      const now = new Date();

      if (now.getHours() < 12) {
        return "Good Morning";
      } else if (now.getHours() < 18) {
        return "Good Afternoon";
      } else {
        return "Good Evening";
      }
    },
  },
  watch: {
    "$store.state.settingsModule.user": {
      handler: function (val, oldVal) {
        this.$data.user = new User(this.getUser);
      },
    },
  },
  async created() {
    this.populateNavMenuItems();

    this.$data.user = new User(this.getUser);
  },
};
</script>
