<template>
  <v-navigation-drawer
    absolute
    app
    color="secondary"
    :value="navDrawerStatus"
    v-if="userLoggedIn"
  >
    <v-list>
      <v-list-item class="top-list-item">
        <v-list-item-content>
          <router-link :to="profileNavigation.url" class="user-profile-item">
            <v-icon x-large color="white">{{ profileNavigation.icon }}</v-icon>
          </router-link>
        </v-list-item-content>
      </v-list-item>
      <v-container class="navigation-status-indicator">
        <router-link :to="profileNavigation.url" class="nav-drawer-item">
          <span class="user-profile-item">{{ user.userName }}</span>
        </router-link>
      </v-container>
      <v-container class="navigation-status-indicator">
        <span class="user-profile-subscript">(Logged In)</span>
      </v-container>
    </v-list>
    <v-list v-if="navMenuItems.length > 1">
      <v-list-item v-for="(navItem, index) in navMenuItems" :key="index">
        <v-list-item-content>
          <v-list-item-title>
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
@media only screen and (max-width: 1265px) {
  .top-list-item {
    margin: 60px 0 0 0;
  }
}
.nav-drawer-item {
  font-weight: bold;
  text-decoration: none !important;
  text-transform: uppercase;
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
</style>
<script>
import User from "@/models/user";
import MenuItem from "@/models/viewModels/menuItem";

export default {
  name: "NavigationBarForm",
  props: ["userLoggedIn", "profileNavigation", "navDrawerStatus"],
  data: () => ({
    navMenuItems: [],
    user: {},
  }),
  methods: {
    populateNavMenuItems() {
      this.$router.options.routes.forEach((route) => {
        if (route.name !== "UserProfile") {
          this.$data.navMenuItems.push(
            new MenuItem(route.path, route.name, "")
          );
        }
      });
    },
  },
  async created() {
    this.populateNavMenuItems();
  },
  beforeUpdate() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
  },
};
</script>