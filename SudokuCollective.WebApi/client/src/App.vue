<template>
  <v-app>
    <v-navigation-drawer app color="secondary" v-if="user.isLoggedIn">
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
          <v-list-item v-if="!user.isLoggedIn">
            <v-list-item-content>
              <v-list-item-title>
                <div class="menu-item" @click="userLoggingIn = true">
                  <v-icon>mdi-login-variant</v-icon>
                  <span class="mr-2">Login</span>
                </div>
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item v-if="user.isLoggedIn">
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
          <hr v-if="appMenuItems.length > 1" />
          <v-list-item v-if="appMenuItems.length > 1">
            <v-menu right>
              <template v-slot:activator="{ on, attrs }">
                <div class="menu-item" v-bind="attrs" v-on="on">
                  <span class="mr-2">Links</span>
                </div>
              </template>
              <v-list>
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

    <!-- Sizes your content based upon application components -->
    <v-main>
      <!-- Provides the application the proper gutter -->
      <v-container fluid>
        <!-- If using vue-router -->
        <transition name="fade">
          <router-view></router-view>
        </transition>

        <v-dialog v-model="userLoggingIn" persistent max-width="600px">
          <LoginForm :userForAuthentication="user" v-on:user-logging-in="login" />
        </v-dialog>
      </v-container>
    </v-main>

    <v-footer app>
      <!-- -->
    </v-footer>
  </v-app>
</template>

<style scoped>
.menu-item {
  text-decoration: none !important;
  color: #9b9b9b;
  cursor: pointer;
}
.nav-drawer-item {
  text-decoration: none !important;
  color: #ffffff;
}
</style>

<script>
import { mapActions } from "vuex";
import { userService } from "../src/services/userService/user.service";
import LoginForm from "../src/components/LoginForm";
import User from "../src/models/user";
import MenuItem from "../src/models/viewModels/menuItem";
import { AppMenuItems } from "../src/models/arrays/appMenuItems";

export default {
  name: "App",

  components: {
    LoginForm,
  },

  data: () => ({
    appMenuItems: [{}],
    navMenuItems: [{}],
    userLoggingIn: false,
    user: {},
  }),

  methods: {
    ...mapActions("appSettingsModule", ["confirmBaseURL"]),

    login(user, token) {
      if (user !== null && token !== null) {
        this.$data.user = user;

        this.$data.user = userService.loginUser(
          this.$data.user,
          token
        );
        
        this.$router.push("/dashboard");
      }

      this.$data.userLoggingIn = false;
    },

    logout() {
      const userFullName = this.$data.user.fullName;

      this.$data.user = userService.logoutUser(this.$data.user);

      if (this.$router.currentRoute.path !== "/") {
        this.$router.push("/");
      }

      this.$toasted.show(`${userFullName} has been logged out.`, {
        duration: 3000,
      });
    },

    populateAppMenuItems() {
      AppMenuItems.forEach((route) => {
        this.$data.appMenuItems.push(route);
      });

      // Drop the first empty item
      this.$data.appMenuItems.shift();
    },

    populateNavMenuItems() {
      this.$router.options.routes.forEach((route) => {
        this.$data.navMenuItems.push(new MenuItem(route.path, route.name, ""));
      });

      // Drop the first empty item
      this.$data.navMenuItems.shift();
    },
  },

  computed: {},

  async created() {
    await this.confirmBaseURL();

    this.populateNavMenuItems();
    this.populateAppMenuItems();
  },

  mounted() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
  },
};
</script>