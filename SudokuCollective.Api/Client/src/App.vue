<template>
  <v-app>
    <NavigationBarForm
      :userLoggedIn="user.isLoggedIn"
      :profileNavigation="profileNavigation" />
    <AppBarForm 
      :userLoggedIn="user.isLoggedIn"
      :profileNavigation="profileNavigation"
      v-on:user-logging-in="userLoggingIn = true"
      v-on:user-logging-out="logout"
      v-on:user-signing-up="userSigningUp = true"/>

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
      <FooterForm />
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
</style>

<script>
import { mapActions } from "vuex";
import { userService } from "@/services/userService/user.service";
import AppBarForm from "@/components/navigation/AppBarForm";
import NavigationBarForm from "@/components/navigation/NavigationBarForm";
import LoginForm from "@/components/forms/LoginForm";
import SignUpForm from "@/components/forms/SignUpForm";
import FooterForm from "@/components/navigation/FooterForm";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions, actionToastOptions } from "@/helpers/toastHelper";

export default {
  name: "App",
  components: {
    AppBarForm,
    NavigationBarForm,
    LoginForm,
    SignUpForm,
    FooterForm
  },
  data: () => ({
    userLoggingIn: false,
    userSigningUp: false,
    user: {},
    profileNavigation: {
      url: "/UserProfile",
      title: "User Profile",
      icon: "mdi-account-circle"
    },
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

            this.$data.user = userService.logoutUser(this.$data.user);

            if (this.$router.currentRoute.path !== "/") {
              this.$router.push("/");
            }

            showToast(
              this, 
              ToastMethods["info"], 
              "You are logged out.",
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
        this.$data.user = user;

        this.$data.user = userService.loginUser(this.$data.user, token);

        if (this.$router.currentRoute.path !== "/dashboard") {
          this.$router.push("/dashboard");
        }

        showToast(
          this, 
          ToastMethods["success"], 
          "Thank you for signing up, please review your email to confirm your address",
          defaultToastOptions()
        );
      }
      this.$data.userSigningUp = false;
    },

    redirectToSignUp() {
      this.$data.userLoggingIn = false;
      this.$data.userSigningUp = true;
    },
  },
  async created() {
    await this.confirmBaseURL();
  },
  mounted() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
  },
};
</script>