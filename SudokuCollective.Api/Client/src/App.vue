<template>
  <v-app :style="cssProps">
    <NavigationBar
      :userLoggedIn="user.isLoggedIn"
      :profileNavigation="profileNavigation"
      :navDrawerStatus="navDrawerStatus"
    />
    <AppBar
      :userLoggedIn="user.isLoggedIn"
      :profileNavigation="profileNavigation"
      :solveNavigation="solveNavigation"
      v-on:user-solving-sudoku="solve"
      v-on:user-logging-in="userLoggingIn = true"
      v-on:user-logging-out="logout"
      v-on:user-signing-up="userSigningUp = true"
      v-on:update-nav-drawer-status="updateNavDrawer"
    />

    <!-- Sizes your content based upon application components -->
    <v-main>
      <!-- Provides the application the proper gutter -->
      <v-container fluid>
        <!-- If using vue-router -->
        <transition name="fade">
          <router-view v-on:user-logging-out="logout"></router-view>
        </transition>

        <v-dialog v-model="userLoggingIn" persistent max-width="600px">
          <LoginForm
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
      <Footer />
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
.button-full {
  width: 100%;
}
</style>

<script>
/* eslint-disable no-unused-vars */
import _ from "lodash";
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { apiURLConfirmationService } from "@/services/apiURLConfirmationService/apiURLConfirmationService";
import { appProvider } from "@/providers/appProvider";
import { userProvider } from "@/providers/userProvider";
import AppBar from "@/components/navigation/AppBar";
import NavigationBar from "@/components/navigation/NavigationBar";
import LoginForm from "@/components/forms/LoginForm";
import SignUpForm from "@/components/forms/SignUpForm";
import Footer from "@/components/navigation/Footer";
import App from "@/models/app";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";
import { convertStringToDateTime } from "@/helpers/commonFunctions/commonFunctions";

export default {
  name: "App",
  components: {
    AppBar,
    NavigationBar,
    LoginForm,
    SignUpForm,
    Footer,
  },
  data: () => ({
    userSolvingSudoku: false,
    userLoggingIn: false,
    userSigningUp: false,
    user: new User(),
    profileNavigation: {
      url: "/UserProfile",
      title: "User Profile",
      icon: "mdi-account-circle",
    },
    solveNavigation: {
      url: "/Solve",
      title: "Solve Sudoku Puzzles",
      icon: "mdi-apps",
    },
    navDrawerStatus: null,
  }),
  methods: {
    ...mapActions("appModule", [
      "updateUsersSelectedApp",
      "removeUsersApps",
      "removeRegisteredApps",
      "updateApps",
      "removeApps"
    ]),
    ...mapActions("settingsModule", [
      "confirmBaseURL",
      "updateAuthToken",
      "updateApp",
      "updateUser",
    ]),
    ...mapActions("userModule", [
      "updateUsers",
      "removeUsers"
    ]),

    async login(user, token) {
      if (user !== null && token !== null) {
        this.userLoginProcess(user, token);

        if (this.$data.user.isSuperUser) {
          
          var userResponse = await userProvider.getUsers();

          var users = [];

          userResponse.users.forEach((u) => {
            const user = new User(u);
            users.push(user);
          });

          this.updateUsers(users);

          var appsResponse = await appProvider.getApps();

          var apps = [];

          appsResponse.apps.forEach((a) => {
            const app = new App(a);
            app["dateCreated"] = convertStringToDateTime(app.dateCreated);
            app["owner"] = _.find(users, function (user) {
              return user.id === app.ownerId;
            })
            apps.push(app);
          });

          this.updateApps(apps);
        }

        let logInMessage;

        if (this.$data.user.emailConfirmed) {
          logInMessage = "You are logged in";
        } else {
          logInMessage = "You are logged in, but please confirm your email";
        }

        showToast(
          this,
          ToastMethods["success"],
          logInMessage,
          defaultToastOptions()
        );
      }

      this.$data.userLoggingIn = false;
    },

    logout() {
      const action = [
        {
          text: "Yes",
          onClick: (e, toastObject) => {
            toastObject.goAway(0);

            this.$data.user = new User();

            this.$data.user.logout();

            this.updateUser(this.$data.user);
            this.updateAuthToken("");
            this.updateUsersSelectedApp(new App());
            this.removeUsersApps();
            this.removeRegisteredApps();
            this.removeApps();
            this.removeUsers();

            if (
              this.$router.currentRoute.path !== "/" &&
              this.$router.currentRoute.path !== "/solve"
            ) {
              this.$router.push("/");
            }

            showToast(
              this,
              ToastMethods["info"],
              "You are logged out",
              defaultToastOptions()
            );
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
        actionToastOptions(action, "logout")
      );
    },

    signUp(user, token) {
      if (user !== null && token !== null) {
        this.userLoginProcess(user, token);

        showToast(
          this,
          ToastMethods["success"],
          "Thank you for signing up, please review your email to confirm your address",
          defaultToastOptions()
        );
      }
      this.$data.userSigningUp = false;
      this.$data.processingAPIRequest = false;
    },

    redirectToSignUp() {
      this.$data.userLoggingIn = false;
      this.$data.userSigningUp = true;
    },

    userLoginProcess(user, token) {
      this.$data.user = user;
      this.$data.user.login();
      this.updateUser(this.$data.user);
      this.updateAuthToken(token);

      if (this.$router.currentRoute.path === "/dashboard") {
        // do nothing...
      } else if (this.$router.currentRoute.path === "/solve") {
        // do nothing...
      } else {
        if (user.emailConfirmed) {
          this.$router.push("/dashboard");
        } else {
          this.$router.push("/userProfile");
        }
      }
    },

    updateNavDrawer() {
      this.$data.navDrawerStatus = this.$data.navDrawerStatus ? false : true;
    },

    solve() {
      console.log("solve invoked...");
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    cssProps() {
      var themeColors = {};
      Object.keys(this.$vuetify.theme.themes.light).forEach((color) => {
        themeColors[`--v-${color}`] = this.$vuetify.theme.themes.light[color];
      });
      return themeColors;
    },
  },
  watch: {
    "$store.state.settingsModule.user": {
      handler: function (val, oldVal) {
        this.$data.user = this.getUser;
      },
    },
  },
  async created() {
    const urlResponse = await apiURLConfirmationService.confirm();

    this.confirmBaseURL(urlResponse.url);

    const app = new App(await appProvider.getByLicense());

    app.updateLicense(process.env.VUE_APP_LICENSE);

    this.updateApp(app);

    this.$data.user = this.getUser;
  },
};
</script>
