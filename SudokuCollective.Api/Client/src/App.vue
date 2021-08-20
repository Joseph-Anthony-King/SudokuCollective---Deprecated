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

        <v-dialog v-model="displayLoginForm" persistent max-width="600px">
          <LoginForm
            :loginFormStatus="userLoggingIn"
            :authExpired="authTokenExpired"
            v-on:user-logging-in-event="login"
            v-on:user-logging-out="logout"
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
import { difficultiesProvider } from "@/providers/difficultiesProvider";
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
    authTokenExpired: false,
    authTokenExtended: false,
    userSigningUp: false,
    user: new User(),
    profileNavigation: {
      url: "/UserProfile",
      title: "User Profile",
      icon: "mdi-account-circle",
    },
    solveNavigation: {
      url: "/Sudoku",
      title: "Sudoku",
      icon: "mdi-puzzle",
    },
    navDrawerStatus: null,
  }),
  methods: {
    ...mapActions("appModule", [
      "updateUsersSelectedApp",
      "updateUsersApps",
      "removeUsersApps",
      "removeRegisteredApps",
      "updateApps",
      "removeApps",
    ]),
    ...mapActions("settingsModule", [
      "confirmBaseURL",
      "updateAuthToken",
      "updateApp",
      "updateUser",
    ]),
    ...mapActions("userModule", ["updateUsers", "removeUsers"]),
    ...mapActions("sudokuModule", [
      "initializePuzzle",
      "initializeGame",
      "updateDifficulties",
      "removeSelectedDifficulty",
    ]),

    async login(user, token) {
      if (user !== null && token !== null) {
        this.userLoginProcess(user, token);

        if (this.$data.user.isSuperUser) {
          const userResponse = await userProvider.getUsers();

          var users = [];

          userResponse.users.forEach((u) => {
            const user = new User(u);
            user["licenses"] = 0;
            users.push(user);
          });

          const appsResponse = await appProvider.getApps();

          var apps = [];

          appsResponse.apps.forEach((a) => {
            const app = new App(a);
            app["owner"] = _.find(users, function (user) {
              return user.id === app.ownerId;
            });
            apps.push(app);
          });

          apps.forEach((app) => {
            users.forEach((user) => {
              if (app.ownerId === user.id) {
                user.licenses++;
              }
            });
          });

          const superUsersAppsResponse = await appProvider.getMyApps();

          var superUsersApps = [];

          superUsersAppsResponse.apps.forEach((a) => {
            const app = new App(a);
            app["owner"] = _.find(users, function (user) {
              return user.id === app.ownerId;
            });
            superUsersApps.push(app);
          });

          this.updateUsers(users);
          this.updateApps(apps);
          this.updateUsersApps(superUsersApps);
        }

        let logInMessage;

        if (this.$data.user.isEmailConfirmed) {
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

      if (this.$data.authTokenExpired) {
        this.$data.authTokenExpired = false;
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

            if (this.$data.authTokenExpired) {
              this.$data.authTokenExpired = false;
            }

            this.$data.userLoggingIn = false;

            this.updateUser(this.$data.user);
            this.updateAuthToken("");
            this.updateUsersSelectedApp(new App());
            this.removeUsersApps();
            this.removeRegisteredApps();
            this.removeApps();
            this.removeUsers();
            this.removeSelectedDifficulty();

            if (
              this.$router.currentRoute.name !== "Home" &&
              this.$router.currentRoute.name !== "Sudoku"
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

      if (this.$router.currentRoute.name === "Dashboard") {
        // do nothing...
      } else if (this.$router.currentRoute.name === "Sudoku") {
        // do nothing...
      } else {
        if (!this.$data.authTokenExtended) {
          if (this.$data.user.isEmailConfirmed) {
            this.$router.push("/dashboard");
          } else {
            this.$router.push("/userProfile");
          }
        } else {
          this.$data.authTokenExtended = false;
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
    ...mapGetters("sudokuModule", ["getDifficulties"]),

    cssProps() {
      var themeColors = {};
      Object.keys(this.$vuetify.theme.themes.light).forEach((color) => {
        themeColors[`--v-${color}`] = this.$vuetify.theme.themes.light[color];
      });
      return themeColors;
    },

    displayLoginForm() {
      if (this.$data.userLoggingIn || this.$data.authTokenExpired) {
        return true;
      } else {
        return false;
      }
    },
  },
  watch: {
    "$store.state.settingsModule.user": {
      handler: function (val, oldVal) {
        this.$data.user = this.getUser;
      },
    },
    "$store.state.settingsModule.authTokenExpired": {
      handler: function (val, oldVal) {
        if (val === true) {
          this.$data.userLoggingIn = val;
          this.$data.authTokenExpired = val;
          this.$data.authTokenExtended = val;
        }
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

    if (this.getDifficulties.length === 0) {
      const difficultiesResponse = await difficultiesProvider.getDifficulties();

      if (difficultiesResponse.success) {
        this.updateDifficulties(difficultiesResponse.difficulties);
      } else {
        showToast(
          this,
          ToastMethods["error"],
          difficultiesResponse.message,
          defaultToastOptions()
        );
      }
    }

    this.initializePuzzle();
    this.initializeGame();
  },
};
</script>
