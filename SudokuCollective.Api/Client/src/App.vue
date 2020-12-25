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
          <v-list-item v-if="!user.isLoggedIn">
            <v-list-item-content>
              <v-list-item-title>
                <div class="menu-item" @click="userSigningUp = true">
                  <v-icon>mdi-account-plus</v-icon>
                  <span class="mr-2">Sign Up</span>
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

    <!-- Sizes your content based upon application components -->
    <v-main>
      <!-- Provides the application the proper gutter -->
      <v-container fluid>
        <!-- If using vue-router -->
        <transition name="fade">
          <router-view></router-view>
        </transition>

        <v-dialog v-model="userLoggingIn" persistent max-width="600px">
          <LoginForm
            :userForAuthentication="user"
            :loginFormStatus="userLoggingIn"
            v-on:user-logging-in-event="login"
            v-on:redirect-to-sign-up="redirectToSignUp"
          />
        </v-dialog>

        <v-dialog v-model="userSigningUp" persistent max-width="600px">
          <SignUpForm
            :signUpFormStatus="userSigningUp"
            v-on:user-signing-up-event="signUp"
          />
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
.menu-link-list {
  width: 215px;
}
.nav-drawer-item {
  font-weight: bold;
  text-decoration: none !important;
  text-transform: uppercase;
  color: #ffffff;
}
</style>

<script>
import { mapActions } from "vuex";
import { userService } from "@/services/userService/user.service";
import LoginForm from "@/components/LoginForm";
import SignUpForm from "@/components/SignUpForm";
import User from "@/models/user";
import MenuItem from "@/models/viewModels/menuItem";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { AppMenuItems } from "@/models/arrays/appMenuItems";
import { showToast } from "@/helpers/toastHelper";

export default {
  name: "App",

  components: {
    LoginForm,
    SignUpForm
  },

  data: () => ({
    appMenuItems: [],
    navMenuItems: [],
    userLoggingIn: false,
    userSigningUp: false,
    user: {},
  }),

  methods: {
    ...mapActions("appSettingsModule", ["confirmBaseURL"]),

    login(user, token) {
      if (user !== null && token !== null) {
        this.$data.user = user;

        this.$data.user = userService.loginUser(this.$data.user, token);

        if (this.$router.currentRoute.path !== "/dashboard") {
          this.$router.push("/dashboard");
        }

        var logInMessage;

        if (this.$data.user.emailConfirmed) {
          logInMessage = "You are logged in";
        } else {
          logInMessage = "You are logged in, but please confirm your email";
        }

        showToast(this, ToastMethods["success"], logInMessage, {
          duration: 3000,
        });
      }

      this.$data.userLoggingIn = false;
    },

    logout() {
      const action = [
        {
          text: "Yes",
          onClick: (e, toastObject) => {
            toastObject.goAway(0);

            this.$data.user = userService.logoutUser(this.$data.user);

            if (this.$router.currentRoute.path !== "/") {
              this.$router.push("/");
            }

            showToast(this, ToastMethods["info"], "You are logged out.", {
              duration: 3000,
            });
          },
        },
        {
          text: "No",
          onClick: (e, toastObject) => {
            toastObject.goAway(0);
          },
        },
      ];

      showToast(
        this,
        ToastMethods["show"],
        "Are you sure you want to log out?",
        { action: action }
      );
    },

    signUp(user, token) {
      if (user !== null && token !== null) {
        this.$data.user = user;

        this.$data.user = userService.loginUser(this.$data.user, token);

        if (this.$router.currentRoute.path !== "/dashboard") {
          this.$router.push("/dashboard");
        }

        showToast(this, ToastMethods["success"], "Thank you for signing up, please confirm your email address", {
          duration: 3000,
        });
      }
      this.$data.userSigningUp = false;
    },

    redirectToSignUp() {
      this.$data.userLoggingIn = false;
      this.$data.userSigningUp = true;
    },

    populateAppMenuItems() {
      AppMenuItems.forEach((route) => {
        this.$data.appMenuItems.push(route);
      });
    },

    populateNavMenuItems() {
      this.$router.options.routes.forEach((route) => {
        this.$data.navMenuItems.push(new MenuItem(route.path, route.name, ""));
      });
    },
  },

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