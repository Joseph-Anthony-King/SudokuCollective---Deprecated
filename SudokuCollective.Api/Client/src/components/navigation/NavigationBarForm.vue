<template>
    <v-navigation-drawer app color="secondary" v-if="userLoggedIn">
      <v-list>
        <v-list-item>
          <v-list-item-content>
              <v-icon x-large color="white">mdi-account-circle</v-icon>   
          </v-list-item-content>
        </v-list-item>
        <v-container class="navigation-status-indicator">
          <span class="user-profile-item">{{ user.userName }}</span> 
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
.nav-drawer-item {
  font-weight: bold;
  text-decoration: none !important;
  text-transform: uppercase;
  color: #ffffff;
}
.user-profile-item {
  font-weight: bold;
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
  props: ["userLoggedIn"],
  data: () => ({
    navMenuItems: [],
    user: {},
  }),
  methods: {

    populateNavMenuItems() {
      this.$router.options.routes.forEach((route) => {
        this.$data.navMenuItems.push(new MenuItem(route.path, route.name, ""));
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
}
</script>