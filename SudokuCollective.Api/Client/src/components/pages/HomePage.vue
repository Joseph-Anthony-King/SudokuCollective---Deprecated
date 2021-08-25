<template>
  <v-container fluid>
    <div class="d-flex ma-12 pa-12 justify-center" v-if="app.name === ''">
      <v-progress-circular
        indeterminate
        color="primary"
        :size="100"
        :width="10"
        class="progress-circular"
      ></v-progress-circular>
    </div>
    <v-card elevation="6" class="mx-auto" v-if="app.name !== ''">
      <v-row class="text-center home-banner">
        <v-col cols="12">
          <v-img src="/images/banner.jpg" height="500" />
          <h1 class="text-center centered-welcome-message text-padding">
            Welcome to {{ app.name }}
          </h1>
          <v-img
            src="/images/logo.png"
            class="my-3 centered-logo"
            contain
            height="200"
          />
        </v-col>
      </v-row>
      <v-row>
        <p class="motto text-center text-padding">
          Code... Create... Inspire...
        </p>
        <p class="description text-center text-padding">
          Sudoku Collective is a project that serves as a ready made Web API
          that you can use to learn client side technologies. The API is
          documented so you can create your own client app which can fully
          integrate with the API. My particular implementation will include
          console and Vue apps.
        </p>
      </v-row>
    </v-card>

    <v-dialog v-model="resetPassword" persistent max-width="600px">
      <ResetPasswordForm
        v-on:password-reset-event="passwordReset"
        v-on:password-reset-form-closed-event="closePasswordResetForm"
        :token="token"
      />
    </v-dialog>
  </v-container>
</template>

<style scoped>
.text-padding {
  padding: 20px 70px 20px 70px;
}
.home-banner {
  position: relative;
  margin-bottom: 0;
  border: 0;
  border-color: transparent;
}
.centered-welcome-message {
  position: absolute;
  top: 10%;
  left: 0;
  width: 100%;
  color: white;
  text-shadow: 2px 2px var(--v-secondary);
}
.centered-logo {
  position: absolute;
  top: 50%;
  left: 0;
  width: 100%;
  filter: drop-shadow(2px 2px 2px var(--v-secondary));
}
.motto {
  margin: auto;
  font-style: italic;
  color: var(--v-secondary);
  font-size: 2em;
}
.description {
  color: var(--v-secondary);
  font-size: 1em;
}
</style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import ResetPasswordForm from "@/components/forms/ResetPasswordForm";
import { userProvider } from "@/providers/userProvider";
import App from "@/models/app";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "HomePage",
  components: {
    ResetPasswordForm,
  },
  data: () => ({
    user: new User(),
    app: new App(),
    resetPassword: false,
    token: null,
  }),
  methods: {
    ...mapActions("settingsModule", ["updateUser"]),

    async confirmEmail(token) {
      try {
        const response = await userProvider.confirmEmail(token);

        if (response.isSuccess) {
          let message = "";

          if (this.$data.user.isLoggedIn) {
            this.$data.user.email = response.email;
            this.$data.user.dateUpdated = new Date(
              response.dateUpdated
            ).toLocaleString();
            this.$data.user.receivedRequestToUpdateEmail = false;
          }

          if (response.message === "Email Confirmed") {
            if (this.$data.user.isLoggedIn) {
              this.$data.user.isEmailConfirmed = true;
              this.$data.user.emailConfirmed = "Yes";
            }
            message = response.message;
          } else if (response.message === "Old Email Confirmed") {
            if (this.$data.user.isLoggedIn) {
              this.$data.user.isEmailConfirmed = false;
              this.$data.user.emailConfirmed = "No";
            }
            message =
              "Please review and confirm your email address:" + response.email;
          } else {
            message = response.message;
          }

          showToast(
            this,
            ToastMethods["success"],
            message,
            defaultToastOptions()
          );

          if (this.$data.user.isLoggedIn) {
            this.updateUser(this.$data.user);
            this.$router.push("/UserProfile");
          }
        } else {
          showToast(
            this,
            ToastMethods["error"],
            response.message,
            defaultToastOptions()
          );
        }
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      }
    },

    async passwordReset() {
      if (this.$data.user.isLoggedIn) {
        const response = await userProvider.getUser(this.$data.user.id);
        this.$data.user = response.user;
        this.$data.user.login();
        this.updateUser(this.$data.user);
      }
    },

    closePasswordResetForm() {
      this.$data.resetPassword = false;

      if (
        this.$data.user.isLoggedIn &&
        !this.$data.user.receivedRequestToUpdatePassword
      ) {
        this.$router.push("/dashboard");
      }
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getApp", "getUser"]),
  },
  watch: {
    "$store.state.settingsModule.app": {
      handler: function (val, oldVal) {
        this.$data.app = this.getApp;
      },
    },
    "$store.state.settingsModule.user": {
      handler: function (val, oldVal) {
        this.$data.user = this.getUser;
      },
    },
  },
  async mounted() {
    this.$data.app = this.getApp;
    this.$data.user = this.getUser;

    if (this.$route.name === "ConfirmEmail" 
      && this.$route.params.token !== undefined) {
      await this.confirmEmail(this.$route.params.token);
    }

    if (this.$route.name === "ResetPassword" 
      && this.$route.params.token !== undefined) {
      this.$data.resetPassword = true;
      this.$data.token = this.$route.params.token;
    }
  },
};
</script>
