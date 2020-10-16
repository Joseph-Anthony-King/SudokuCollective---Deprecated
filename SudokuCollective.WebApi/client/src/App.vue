<template>
  <v-app>
    <v-navigation-drawer app color="secondary">
      <v-list>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title>
              <router-link to="/" class="nav-drawer-item">HOME</router-link>
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
      <v-list>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title>
              <router-link to="/Dashboard" class="nav-drawer-item">Dashboard</router-link>
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
          <v-list-item v-for="(menuItem, index) in menuItems" :key="index">
            <v-list-item-content>
              <v-list-item-title>
                <div v-if="!menuItem.isSitePage">
                  <a
                    :href="menuItem.url"
                    target="blank"
                    :title="menuItem.text"
                    class="menu-item"
                  >
                    <v-icon>{{ menuItem.mdiIcon }}</v-icon>
                    <span class="mr-2">{{ menuItem.text }}</span>
                  </a>
                </div>
                <div v-if="menuItem.isSitePage">
                  <div class="menu-item" @click="dialog = true">
                    <v-icon>{{ menuItem.mdiIcon }}</v-icon>
                    <span class="mr-2">{{ menuItem.text }}</span>
                  </div>
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
              <v-btn color="blue darken-1" text @click="login()">
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
    menuItems: [{}],
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

          alert(`${this.$data.user.fullName} is logged in.`);
        } else if (response.status === 400) {
          alert("Username or Password is incorrect.");
        } else {
          alert("An error occurred while trying to authenticate the user");
        }

        this.$data.dialog = false;
        this.$data.username = "";
        this.$data.password = "";

      } catch (error) {
        alert(error);
      }
    },

    populateMenuItems() {
      this.$data.menuItems.push(
        new MenuItem(
          this.$store.getters["appSettingsModule/getApiURL"],
          "API Status",
          "mdi-apps",
          false,
          false
        )
      );

      this.$data.menuItems.push(
        new MenuItem(
          `${this.apiUrl}/swagger/index.html`,
          "API Documentation",
          "mdi-open-in-new",
          false,
          false
        )
      );

      this.$data.menuItems.push(
        new MenuItem(
          "https://github.com/Joseph-Anthony-King/SudokuCollective",
          "GitHub Page",
          "mdi-github",
          false,
          false
        )
      );

      this.$data.menuItems.push(
        new MenuItem("", "Login", "mdi-login-variant", true, false)
      );

      // Drop the first empty item
      this.$data.menuItems.shift();

      console.log("menuItems:", this.$data.menuItems);
    }
  },

  async created() {
    await this.confirmBaseURL();

    this.apiUrl = this.$store.getters["appSettingsModule/getApiURL"];
    this.apiDocumentationUrl = `${this.apiUrl}/swagger/index.html`;

    this.populateMenuItems();
  },

  mounted() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.getUser);
  }
};
</script>