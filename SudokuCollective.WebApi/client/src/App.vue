<template>
  <v-app>
    <v-navigation-drawer app color="secondary" v-if="user.isLoggedIn">
      <v-list v-if="navMenuItems.length > 1">
        <v-list-item v-for="(navItem, index) in navMenuItems" :key="index">
          <v-list-item-content>
            <v-list-item-title>
              <router-link :to="navItem.url" class="nav-drawer-item">{{
                navItem.text
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
        <v-list v-if="appMenuItems.length > 1">
          <!-- outside links -->
          <v-list-item v-for="(menuItem, index) in appMenuItems" :key="index">
            <v-list-item-content>
              <v-list-item-title>
                <a
                  :href="menuItem.url"
                  target="blank"
                  :title="menuItem.text"
                  class="menu-item"
                >
                  <v-icon>{{ menuItem.mdiIcon }}</v-icon>
                  <span class="mr-2">{{ menuItem.text }}</span>
                </a>
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <!-- User Functionality -->
          <v-list-item v-if="!user.isLoggedIn">
            <v-list-item-content>
              <v-list-item-title>
                <div class="menu-item" @click="dialog = true">
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

        <v-dialog v-model="dialog" persistent max-width="600px">
          <v-card>
            <v-card-title>
              <span class="headline">Login</span>
            </v-card-title>
            <v-card-text>
              <v-container>
                <v-row>
                  <v-col cols="12">
                    <v-text-field
                      v-model="username"
                      label="User Name"
                      required
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12">
                    <v-text-field
                      v-model="password"
                      label="Password"
                      type="password"
                      required
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="dialog = false">
                Close
              </v-btn>
              <v-btn color="blue darken-1" text @click="login">
                Login
              </v-btn>
            </v-card-actions>
          </v-card>
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
import { authenticationService } from "../src/services/authenticationService/authentication.service";
import User from "../src/models/user";
import MenuItem from "../src/models/viewModels/menuItem";

export default {
  name: "App",

  data: () => ({
    apiUrl: "",
    apiDocumentationUrl: "",
    appMenuItems: [{}],
    navMenuItems: [{}],
    dialog: false,
    username: "",
    password: "",
    user: {}
  }),

  methods: {
    ...mapActions("appSettingsModule", ["confirmBaseURL"]),

    async login() {
      try {
        const response = await authenticationService.authenticateUser(
          this.$data.username,
          this.$data.password
        );

        if (response.status === 200) {
          this.$data.user.shallowClone(response.data.user);

          this.$data.user = userService.loginUser(
            this.$data.user,
            response.data.token
          );

          await this.resetData();

          this.$data.dialog = false;
          this.$router.push("/dashboard");

          this.$snack.success(`${this.$data.user.fullName} is logged in.`);
        } else if (response.status === 400) {
          this.$snack.success("Username or Password is incorrect.");
        } else {
          this.$snack.success(
            "An error occurred while trying to authenticate the user"
          );
        }
      } catch (error) {
        alert(error);
      }
    },

    async logout() {
      await this.resetData();

      const userFullName = this.$data.user.fullName;

      this.$data.user = userService.logoutUser(this.$data.user);

      this.$router.push("/");

      this.$snack.success(`${userFullName} has been logged out.`);
    },

    async resetData() {
      this.$data.username = "";
      this.$data.password = "";
    },

    populateAppMenuItems() {
      this.$data.appMenuItems.push(
        new MenuItem(
          this.$store.getters["appSettingsModule/getApiURL"],
          "API Status",
          "mdi-apps"
        )
      );

      this.$data.appMenuItems.push(
        new MenuItem(
          `${this.apiUrl}/swagger/index.html`,
          "API Documentation",
          "mdi-open-in-new"
        )
      );

      this.$data.appMenuItems.push(
        new MenuItem(
          "https://github.com/Joseph-Anthony-King/SudokuCollective",
          "GitHub Page",
          "mdi-github"
        )
      );

      // Drop the first empty item
      this.$data.appMenuItems.shift();
    },

    populateNavMenuItems() {
      this.$router.options.routes.forEach(route => {
        this.$data.navMenuItems.push(new MenuItem(route.path, route.name, ""));
      });

      // Drop the first empty item
      this.$data.navMenuItems.shift();
    }
  },

  computed: {},

  async created() {
    await this.confirmBaseURL();

    this.apiUrl = this.$store.getters["appSettingsModule/getApiURL"];
    this.apiDocumentationUrl = `${this.apiUrl}/swagger/index.html`;

    this.populateNavMenuItems();
    this.populateAppMenuItems();
  },

  mounted() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
  }
};
</script>