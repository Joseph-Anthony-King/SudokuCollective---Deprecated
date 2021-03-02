<template>
  <v-card>
    <v-card-title v-show="!gettingHelp">
      <span class="headline">Login</span>
    </v-card-title>
    <v-card-title v-show="gettingHelp">
      <span class="headline">Login Help</span>
    </v-card-title>
    <v-form v-model="loginFormIsValid" ref="loginForm" v-show="!gettingHelp">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="username"
                label="User Name"
                prepend-icon="mdi-account-circle"
                :rules="userNameRules"
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="password"
                label="Password"
                :type="showPassword ? 'text' : 'password'"
                prepend-icon="mdi-lock"
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append="showPassword = !showPassword"
                :rules="passwordRules"
                autocomplete="new-password"
                required
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="gettingHelp = true"
              v-show="needHelp"
              v-bind="attrs"
              v-on="on"
            >
              Help
            </v-btn>
          </template>
          <span
            >Having trouble logging in? If your email has been confirmed you can
            reset your password or confirm your user name</span
          >
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="redirectToSignUp"
              v-show="needHelp"
              v-bind="attrs"
              v-on="on"
            >
              Sign Up
            </v-btn>
          </template>
          <span>Please sign up if you haven't done so already</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="reset"
              v-bind="attrs"
              v-on="on"
            >
              Reset
            </v-btn>
          </template>
          <span>Reset the login form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="close"
              v-bind="attrs"
              v-on="on"
            >
              Close
            </v-btn>
          </template>
          <span>Close the login form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="submit"
              :disabled="!loginFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Login
            </v-btn>
          </template>
          <span>Submit the login form</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
    <v-form
      v-model="userNameFormIsValid"
      ref="userNameForm"
      v-show="gettingHelp"
    >
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="email"
                label="Please Confirm Your Email"
                prepend-icon="mdi-email"
                required
                :rules="emailRules"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="requestPasswordReset"
              :disabled="!userNameFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Reset Password
            </v-btn>
          </template>
          <span
            >Send a link to your email to reset your password if your email has
            been confirmed</span
          >
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="confirmUserName"
              :disabled="!userNameFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Confirm User Name
            </v-btn>
          </template>
          <span>Obtain your user name if your email has been confirmed</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="gettingHelp = false"
              v-bind="attrs"
              v-on="on"
            >
              Go Back
            </v-btn>
          </template>
          <span>Go back to the login form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="close"
              v-bind="attrs"
              v-on="on"
            >
              Close
            </v-btn>
          </template>
          <span>Close the login form</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
import { authenticationService } from "@/services/authenticationService/authentication.service";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";
import { passwordReset } from "@/helpers/commonFunctions/commonFunctions";
import isChrome from "@/mixins/isChrome";

export default {
  name: "LoginForm",
  props: ["loginFormStatus"],
  mixins: [isChrome],
  data: () => ({
    username: "",
    password: "",
    email: "",
    user: new User(),
    loginFormIsValid: true,
    userNameFormIsValid: true,
    showPassword: false,
    needHelp: false,
    gettingHelp: false,
    invalidUserNames: [],
    invalidPasswords: [],
    invalidEmails: [],
  }),
  methods: {
    async submit() {
      if (this.getLoginFormStatus) {
        try {
          const response = await authenticationService.authenticateUser(
            this.$data.username,
            this.$data.password
          );

          if (response.status === 200) {
            this.$data.user = new User(response.data.user);

            this.reset();

            this.$emit(
              "user-logging-in-event",
              this.$data.user,
              response.data.token
            );
          } else if (response.status === 400) {
            if (
              response.data === "Status Code 400: No User Has This User Name"
            ) {
              this.$data.invalidUserNames.push(this.$data.username);
              this.$refs.loginForm.validate();
              this.$data.needHelp = true;
              showToast(
                this,
                ToastMethods["error"],
                response.data.substring(17),
                defaultToastOptions()
              );
            } else if (response.data === "Status Code 400: Password Invalid") {
              this.$data.invalidPasswords.push(this.$data.password);
              this.$refs.loginForm.validate();
              this.$data.needHelp = true;
              showToast(
                this,
                ToastMethods["error"],
                response.data.substring(17),
                defaultToastOptions()
              );
            } else {
              this.$data.needHelp = true;
              showToast(
                this,
                ToastMethods["error"],
                response.data,
                defaultToastOptions()
              );
            }
          } else {
            this.$data.needHelp = true;
            showToast(
              this,
              ToastMethods["error"],
              "An error occurred while trying to authenticate the user",
              defaultToastOptions()
            );
          }
        } catch (error) {
          this.$data.needHelp = true;
          showToast(this, ToastMethods["error"], error, defaultToastOptions());
        }
      }
    },

    async confirmUserName() {
      try {
        const response = await authenticationService.confirmUserName(
          this.$data.email
        );

        if (response.status === 200) {
          this.$data.username = response.data.userName;

          showToast(
            this,
            ToastMethods["success"],
            "Your user name has been retrieved",
            defaultToastOptions()
          );

          this.$data.gettingHelp = false;
        } else {
          showToast(
            this,
            ToastMethods["error"],
            response.data.message.substring(17),
            defaultToastOptions()
          );
        }
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      }
    },

    requestPasswordReset() {
      let result = passwordReset(this.$data.email, this);

      if (result) {
        this.$data.gettingHelp = false;
      }
    },

    redirectToSignUp() {
      this.reset();

      this.$emit("redirect-to-sign-up", null, null);
    },

    reset() {
      this.$data.needHelp = false;
      this.$refs.loginForm.reset();
      this.$refs.userNameForm.reset();
      document.activeElement.blur();
    },

    close() {
      this.reset();
      this.$emit("user-logging-in-event", null, null);
    },
  },
  computed: {
    userNameRules() {
      return [
        (v) => !!v || "User Name is required",
        (v) =>
          !this.$data.invalidUserNames.includes(v) ||
          "No User Has This User Name",
      ];
    },

    passwordRules() {
      return [
        (v) => !!v || "Password is required",
        (v) => !this.$data.invalidPasswords.includes(v) || "Password Invalid",
      ];
    },

    emailRules() {
      return [
        (v) => !!v || "Email is required",
        (v) =>
          !v ||
          /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) ||
          "Email must be valid",
        (v) =>
          !this.$data.invalidEmails.includes(v) ||
          `Sudoku Collective does not have a record for ${this.$data.email}, please review the sign up feature to establish an account.`,
      ];
    },

    getLoginFormStatus() {
      return this.loginFormStatus && !this.$data.gettingHelp;
    },

    resetLoginFormStatus() {
      return !this.loginFormStatus;
    },
  },
  mounted() {
    if (this.$props.loginFormStatus) {
      let self = this;
      window.addEventListener("keyup", function (event) {
        if (event.key === "Enter" && self.$props.loginFormStatus) {
          self.submit();
        }
      });
    }
  },
};
</script>
