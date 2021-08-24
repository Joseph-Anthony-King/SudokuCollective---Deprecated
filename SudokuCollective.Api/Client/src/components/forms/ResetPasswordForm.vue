<template>
  <v-card>
    <v-card-title>
      <span class="headline">Reset Password</span>
    </v-card-title>
    <v-form v-model="resetPasswordFormIsValid" ref="resetPasswordForm">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="newPassword"
                label="New Password"
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
              v-bind="attrs"
              v-on="on"
              :disabled="!resetPasswordFormIsValid"
            >
              Reset Password
            </v-btn>
          </template>
          <span>Reset Password</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import ResetPasswordModel from "@/models/viewModels/ResetPasswordModel";
import { userProvider } from "@/providers/userProvider";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";
import isChrome from "@/mixins/isChrome";

export default {
  name: "ResetPasswordForm",
  props: ["resetPasswordFormStatus", "token"],
  mixins: [isChrome],
  data: () => ({
    resetPasswordFormIsValid: true,
    newPassword: "",
    confirmPassword: "",
    showPassword: false,
  }),
  methods: {
    ...mapActions("settingsModule", ["updateProcessing"]),

    async submit() {
      var data = new ResetPasswordModel(
        this.$props.token,
        this.$data.newPassword
      );

      var response = await userProvider.resetPassword(data);

      if (response.isSuccess) {
        this.$emit("password-reset-event", null, null);

        showToast(
          this,
          ToastMethods["success"],
          response.message,
          defaultToastOptions()
        );

        this.close();
      } else {
        showToast(
          this,
          ToastMethods["error"],
          response.message,
          defaultToastOptions()
        );
      }
    },

    close() {
      this.reset();
      this.$emit("password-reset-form-closed-event", null, null);
    },

    reset() {
      this.$refs.resetPasswordForm.reset();
      document.activeElement.blur();
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUserName"]),

    stringRequiredRules() {
      return [(v) => !!v || "Value is required"];
    },

    confirmPasswordRules() {
      return [
        (v) => !!v || "Value is required",
        () =>
          this.$data.confirmPassword === this.$data.newPassword ||
          "Passwords do not match",
      ];
    },
  },
  mounted() {
    this.updateProcessing(true);
    this.updateProcessing(false);
  },
};
</script>
