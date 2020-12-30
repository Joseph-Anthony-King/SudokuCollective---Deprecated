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
                required
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          color="blue darken-1"
          text
          @click="gettingHelp = true"
          v-show="needHelp"
        >
          Help
        </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="redirectToSignUp"
          v-show="needHelp"
        >
          Sign Up
        </v-btn>
        <v-btn color="blue darken-1" text @click="resetForm"> Reset </v-btn>
        <v-btn color="blue darken-1" text @click="close"> Close </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="authenticate"
          :disabled="!loginFormIsValid"
        >
          Login
        </v-btn>
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
        <v-btn
          color="blue darken-1"
          text
          @click="requestPasswordReset"
          :disabled="!userNameFormIsValid"
        >
          Reset Password
        </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="confirmUserName"
          :disabled="!userNameFormIsValid"
        >
          Confirm User Name
        </v-btn>
        <v-btn color="blue darken-1" text @click="close"> Close </v-btn>
        <v-btn color="blue darken-1" text @click="gettingHelp = false">
          Go Back
        </v-btn>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
import { authenticationService } from "@/services/authenticationService/authentication.service";
import { userService } from "@/services/userService/user.service";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "LoginForm",
  props: ["userForAuthentication", "loginFormStatus"],
  data: () => ({
    username: "",
    password: "",
    email: "",
    user: {},
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
    async authenticate() {
      if (this.getLoginFormStatus) {
        try {
          const response = await authenticationService.authenticateUser(
            this.$data.username,
            this.$data.password
          );

          if (response.status === 200) {
            this.$data.user.shallowClone(response.data.user);

            this.resetLoginFormStatus;

            this.resetForm();

            this.$emit(
              "user-logging-in-event",
              this.$data.user,
              response.data.token
            );
          } else if (response.status === 400) {
            if (response.data === "Status Code 400: No User Has This User Name") {
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
          showToast(
            this, 
            ToastMethods["error"], 
            error,
            defaultToastOptions()
          );
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
        showToast(
          this, 
          ToastMethods["error"], 
          error,
          defaultToastOptions()
        );
      }
    },

    async requestPasswordReset() {
      try {
        const response = await userService.getRequestPasswordReset(
          this.$data.email
        );

        if (response.status === 200) {
        
          showToast(
            this,
            ToastMethods["success"],
            `Please review ${this.$data.email} to reset your password`,
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
        showToast(
          this, 
          ToastMethods["error"], 
          error,
          defaultToastOptions()
        );
      }
    },

    resetForm() {
      this.$refs.loginForm.reset();
      this.$refs.userNameForm.reset();
      this.$data.invalidUserNames = [];
      this.$data.invalidPasswords = [];
      this.$data.showPassword = false;
      this.$data.needHelp = false;
      this.$data.gettingHelp = false;
    },

    redirectToSignUp() {
      this.$emit("redirect-to-sign-up", null, null);

      this.resetForm();
    },

    close() {
      this.$emit("user-logging-in-event", null, null);

      this.resetForm();
    },
  },
  computed: {
    userNameRules() {
      return [
        (v) => !!v || "User Name is required",
        (v) =>
          !this.$data.invalidUserNames.includes(v) ||
          this.$data.invalidUserNameMessage,
      ];
    },

    passwordRules() {
      return [
        (v) => !!v || "Password is required",
        (v) =>
          !this.$data.invalidPasswords.includes(v) ||
          this.$data.invalidPasswordMessage,
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
    let self = this;

    window.addEventListener("keyup", function (event) {
      if (event.keyCode === 13) {
        self.authenticate();
      }
    });

    this.$data.user = new User();
    this.$data.user.shallowClone(this.$props.userForAuthentication);
  },
};
</script>