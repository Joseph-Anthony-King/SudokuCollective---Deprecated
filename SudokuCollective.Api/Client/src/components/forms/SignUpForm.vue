<template>
  <v-card>
    <v-card-title>
      <span class="headline">Sign Up</span>
    </v-card-title>
    <v-overlay :value="processing">
      <v-progress-circular
        indeterminate
        color="primary"
        :size="100"
        :width="10"
        class="progress-circular"
      ></v-progress-circular>
    </v-overlay>
    <v-form v-model="signUpFormIsValid" ref="signUpForm">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="username"
                label="User Name"
                prepend-icon="mdi-account-plus"
                :rules="userNameRules"
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="firstname"
                label="First Name"
                prepend-icon="mdi-account-plus"
                :rules="stringRequiredRules"
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="lastname"
                label="Last Name"
                prepend-icon="mdi-account-plus"
                :rules="stringRequiredRules"
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="nickname"
                label="Nickname (Not Required)"
                prepend-icon="mdi-account-plus"
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="email"
                label="Email"
                prepend-icon="mdi-email"
                required
                :rules="emailRules"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="password"
                label="Password"
                :type="showPassword ? 'text' : 'password'"
                prepend-icon="mdi-account-key"
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append="showPassword = !showPassword"
                :rules="stringRequiredRules"
                autocomplete="new-password"
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="confirmPassword"
                label="Confirm Password"
                :type="showPassword ? 'text' : 'password'"
                prepend-icon="mdi-account-key"
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append="showPassword = !showPassword"
                :rules="confirmPasswordRules"
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
              @click="reset"
              v-bind="attrs"
              v-on="on"
            >
              Reset
            </v-btn>
          </template>
          <span>Reset the sign up form</span>
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
          <span>Close the sign up form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="submit"
              :disabled="!signUpFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Sign Up
            </v-btn>
          </template>
          <span>Submit the sign up form</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { registerService } from "@/services/registerService/registerService";
import SignUpModel from "@/models/viewModels/signUpModel";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";
import isChrome from "@/mixins/isChrome";

export default {
  name: "SignUpForm",
  props: ["signUpFormStatus"],
  mixins: [isChrome],
  data: () => ({
    username: "",
    firstname: "",
    lastname: "",
    nickname: "",
    email: "",
    password: "",
    confirmPassword: "",
    signUpFormIsValid: true,
    showPassword: false,
    processing: false,
    user: new User(),
    invalidUserNames: [],
    invalidEmails: [],
  }),
  methods: {
    ...mapActions("settingsModule", ["updateUserName"]),

    async submit() {
      if (this.getSignUpFormStatus) {
        try {
          this.$data.processing = true;

          const response = await registerService.postSignUp(
            new SignUpModel(
              this.$data.username,
              this.$data.firstname,
              this.$data.lastname,
              this.$data.nickname,
              this.$data.email,
              this.$data.password
            )
          );

          if (response.status === 201) {
            this.$data.user = new User(response.data.user);

            this.resetSignUpFormStatus;

            this.reset();

            this.updateUserName("");

            this.$emit(
              "user-signing-up-event",
              this.$data.user,
              response.data.token
            );
          } else if (response.status === 404) {
            if (
              response.data ===
              "Status Code 404: User Name Accepts Alphanumeric And Special Characters Except Double And Single Quotes"
            ) {
              this.$data.invalidUserNames.push(this.$data.username);
              showToast(
                this,
                ToastMethods["error"],
                response.data.message.substring(17),
                defaultToastOptions()
              );
            } else if (
              response.data === "Status Code 404: User Name Not Unique"
            ) {
              this.$data.invalidPasswords.push(this.$data.username);
              showToast(
                this,
                ToastMethods["error"],
                response.data.message.substring(17),
                defaultToastOptions()
              );
            } else if (response.data === "Status Code 404: Email Not Unique") {
              this.$data.invalidEmails.push(this.$data.email);
              showToast(
                this,
                ToastMethods["error"],
                response.data.message.substring(17),
                defaultToastOptions()
              );
            } else {
              showToast(
                this,
                ToastMethods["error"],
                response.data.message.substring(17),
                defaultToastOptions()
              );
            }
          } else {
            showToast(
              this,
              ToastMethods["error"],
              response.data.message,
              defaultToastOptions()
            );
          }

          this.$data.processing = false;
        } catch (error) {
          showToast(this, ToastMethods["error"], error, defaultToastOptions());
        }
      }
    },

    reset() {
      this.$refs.signUpForm.reset();
      document.activeElement.blur();
    },

    close() {
      this.reset();
      this.$emit("user-signing-up-event", null, null);
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUserName"]),

    userNameRules() {
      return [
        (v) => !!v || "User Name is required",
        (v) =>
          !this.$data.invalidUserNames.includes(v) || "User Name is not unique",
      ];
    },

    stringRequiredRules() {
      return [(v) => !!v || "Value is required"];
    },

    confirmPasswordRules() {
      return [
        (v) => !!v || "Value is required",
        () =>
          this.$data.confirmPassword === this.$data.password ||
          "Passwords do not match",
      ];
    },

    emailRules() {
      return [
        (v) => !!v || "Email is required",
        (v) =>
          !v ||
          /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) ||
          "Email must be valid",
        (v) => !this.$data.invalidEmails.includes(v) || `Email is not unique`,
      ];
    },

    resetSignUpFormStatus() {
      return !this.signUpFormStatus;
    },

    getSignUpFormStatus() {
      return this.signUpFormStatus;
    },
  },
  watch: {
    "$store.state.settingsModule.userName": {
      handler: function (val, oldVal) {
        this.$data.username = this.getUserName;
      },
    },
  },
  mounted() {
    const username = this.getUserName;

    if (username !== "") {
      this.$data.username = username;
    }

    if (this.$props.signUpFormStatus) {
      let self = this;
      window.addEventListener("keyup", function (event) {
        if (event.key === "Enter" && self.$data.signUpFormIsValid) {
          self.submit();
        }
      });
    }
  },
};
</script>
