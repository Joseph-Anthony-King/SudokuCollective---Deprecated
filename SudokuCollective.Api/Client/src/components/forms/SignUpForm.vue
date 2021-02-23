<template>
  <v-card>
    <v-card-title>
      <span class="headline">Sign Up</span>
    </v-card-title>
    <v-form v-model="signUpFormIsValid" ref="signUpForm">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="username"
                label="User Name"
                prepend-icon="mdi-account-plus"
                :rules="stringRequiredRules"
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
              @click="resetForm"
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
              @click="register"
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
import { registerService } from "@/services/registerService/register.service";
import SignUpModel from "@/models/viewModels/signUpModel";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "SignUpForm",
  props: ["signUpFormStatus"],
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
    user: {},
  }),
  methods: {
    async register() {
      if (this.getSignUpFormStatus) {
        try {
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
            this.$data.user.shallowClone(response.data.user);

            this.resetSignUpFormStatus;

            this.resetForm();

            this.$emit(
              "user-signing-up-event",
              this.$data.user,
              response.data.token
            );
          } else {
            showToast(
              this,
              ToastMethods["error"],
              response.data,
              defaultToastOptions()
            );
          }
        } catch (error) {
          showToast(this, ToastMethods["error"], error, defaultToastOptions());
        }
      }
    },

    resetForm() {
      this.$refs.signUpForm.reset();
      this.$data.showPassword = false;
    },

    close() {
      this.$emit("user-signing-up-event", null, null);
      setTimeout(this.resetForm(), 10000);
    },
  },
  computed: {
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
      ];
    },

    resetSignUpFormStatus() {
      return !this.signUpFormStatus;
    },

    getSignUpFormStatus() {
      return this.signUpFormStatus;
    },
  },
  mounted() {
    this.$data.user = new User();
  },
};
</script>
