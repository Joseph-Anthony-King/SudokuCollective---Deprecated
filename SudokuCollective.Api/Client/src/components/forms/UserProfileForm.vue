<template>
  <div>
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title v-if="user.isActive" class="justify-center"
            >Your Account is Active</v-card-title
          >
          <v-card-title v-if="!user.isActive" class="justify-center warning"
            >Your Account is Deativated, Please Contact the
            Administrator</v-card-title
          >
          <hr class="title-spacer" />
          <UserProfileWidget :user="user" />
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
    <v-card elevation="6">
      <v-card-title
        v-if="
          !user.receivedRequestToUpdateEmail &&
          !user.receivedRequestToUpdatePassword &&
          user.emailConfirmed
        "
        class="justify-center success"
        >No Outstanding Requests for this Account</v-card-title
      >
      <v-card-title v-if="!user.emailConfirmed" class="justify-center warning"
        >Please Confirm Your Email: {{ user.email }}</v-card-title
      >
      <v-card-title
        v-if="user.receivedRequestToUpdateEmail"
        class="justify-center warning"
        >Please Review Your Old Email To Begin Update Process:
        {{ user.email }}</v-card-title
      >
      <v-card-title
        v-if="user.receivedRequestToUpdatePassword"
        class="justify-center warning"
        >Received Request to Update Password</v-card-title
      >
    </v-card>
    <hr />
    <v-card elevation="6">
      <v-card-title v-if="user.isActive" class="justify-center"
        >Available Actions</v-card-title
      >
      <v-card-actions>
        <v-container>
          <v-row dense>
            <v-col v-if="!user.isAdmin && user.emailConfirmed">
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="obtainAdminPrivileges"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Obtain Admin Privileges
                  </v-btn>
                </template>
                <span>Add admin privileges to your account</span>
              </v-tooltip>
            </v-col>
            <v-col>
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="refresh"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Refresh
                  </v-btn>
                </template>
                <span>Get your latest profile data from the api</span>
              </v-tooltip>
            </v-col>
            <v-col>
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="editProfile"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Edit Profile
                  </v-btn>
                </template>
                <span>Edit your profile</span>
              </v-tooltip>
            </v-col>
            <v-col
              v-if="!user.receivedRequestToUpdateEmail && !user.emailConfirmed"
            >
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="resendEmailConfirmation"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Resend Email Confirmation
                  </v-btn>
                </template>
                <span
                  >Resend the email confirmation to confirm your new email</span
                >
              </v-tooltip>
            </v-col>
            <v-col
              v-if="
                user.receivedRequestToUpdateEmail ||
                (!user.emailConfirmed &&
                  user.dateUpdated !== '0001-01-01T00:00:00Z')
              "
            >
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="cancelEmailConfirmation"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Cancel Email Confirmation
                  </v-btn>
                </template>
                <span>Cancel your email confirmation request</span>
              </v-tooltip>
            </v-col>
            <v-col
              v-if="
                !user.receivedRequestToUpdatePassword && user.emailConfirmed
              "
            >
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="resetPassword"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Reset Password
                  </v-btn>
                </template>
                <span>Send a link to your email to reset your password</span>
              </v-tooltip>
            </v-col>
            <v-col v-if="user.receivedRequestToUpdatePassword">
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="resendResetPassword"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Resend Reset Password
                  </v-btn>
                </template>
                <span>Resend the password reset request to your email</span>
              </v-tooltip>
            </v-col>
            <v-col v-if="user.receivedRequestToUpdatePassword">
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="cancelResetPassword"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Cancel Reset Password
                  </v-btn>
                </template>
                <span>Cancel your password reset request</span>
              </v-tooltip>
            </v-col>
            <v-col
              v-if="
                (user.receivedRequestToUpdateEmail ||
                  (!user.emailConfirmed &&
                    user.dateUpdated !== '0001-01-01T00:00:00Z')) &&
                user.receivedRequestToUpdatePassword
              "
            >
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="blue darken-1"
                    text
                    @click="cancelAllEmailRequests"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Cancel All Email Requests
                  </v-btn>
                </template>
                <span
                  >Cancel your email confirmation and password reset
                  request</span
                >
              </v-tooltip>
            </v-col>
            <v-col v-if="!user.isSuperUser">
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    class="button-full"
                    color="red darken-1"
                    text
                    @click="deleteUser"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Delete
                  </v-btn>
                </template>
                <span
                  >Delete your profile and all information collective wide</span
                >
              </v-tooltip>
            </v-col>
          </v-row>
        </v-container>
      </v-card-actions>
    </v-card>
  </div>
</template>

<style scoped>
.success {
  color: white;
}

.warning {
  color: red;
}
</style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { userProvider } from "@/providers/userProvider";
import { registerService } from "@/services/registerService/registerService";
import { appProvider } from "@/providers/appProvider";
import UserProfileWidget from "@/components/widgets/UserProfileWidget";
import App from "@/models/app";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";
import { passwordReset } from "@/helpers/commonFunctions/commonFunctions";

export default {
  name: "UserProfileForm",
  components: {
    UserProfileWidget,
  },
  data: () => ({
    user: new User(),
  }),
  methods: {
    ...mapActions("appModule", ["updateUsersSelectedApp", "removeUsersApps"]),
    ...mapActions("settingsModule", ["updateAuthToken", "updateUser"]),

    editProfile() {
      this.$emit("edit-profile-event", null, null);
    },

    async resendEmailConfirmation() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await registerService.putResendEmailConfirmation();

              if (response.status === 200) {
                await this.reset();
                showToast(
                  this,
                  ToastMethods["success"],
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
            } catch (error) {
              showToast(
                this,
                ToastMethods["error"],
                error,
                defaultToastOptions()
              );
            }
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
        "Are you sure you want to resend your email confirmation request?",
        actionToastOptions(action, "email")
      );
    },

    async cancelEmailConfirmation() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await userProvider.cancelEmailConfirmation();

              if (response.status === 200) {
                await this.reset();
                showToast(
                  this,
                  ToastMethods["success"],
                  response.message,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  response.message,
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
        "Are you sure you want to cancel your email confirmation request?",
        actionToastOptions(action, "email")
      );
    },

    async resetPassword() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              let result = await passwordReset(this.$data.user.email, this);

              if (result) {
                await this.reset();
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
        "Are you sure you want to reset your password?",
        actionToastOptions(action, "lock")
      );
    },

    async resendResetPassword() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await userProvider.resendPasswordReset();

              if (response.status === 200) {
                await this.reset();
                showToast(
                  this,
                  ToastMethods["success"],
                  response.message,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  response.message,
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
        "Are you sure you want to resend your password reset request?",
        actionToastOptions(action, "lock")
      );
    },

    async cancelResetPassword() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await userProvider.cancelPasswordReset();

              if (response.status === 200) {
                await this.reset();
                showToast(
                  this,
                  ToastMethods["success"],
                  response.message,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  response.message,
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
        "Are you sure you want to cancel your reset password request?",
        actionToastOptions(action, "lock")
      );
    },

    async cancelAllEmailRequests() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await userProvider.cancelAllEmailRequests();

              if (response.status === 200) {
                await this.reset();
                showToast(
                  this,
                  ToastMethods["success"],
                  response.message,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  response.message,
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
        "Are you sure you want to cancel all email request?",
        actionToastOptions(action, "mode_edit")
      );
    },

    async obtainAdminPrivileges() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await appProvider.putActivateAdminPrivileges(
                this.getAppId,
                this.$data.user.id
              );

              if (response.status === 200) {
                await this.reset();
                showToast(
                  this,
                  ToastMethods["success"],
                  response.message,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  response.message,
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
        "Are you sure you want to add admin privileges to your account?",
        actionToastOptions(action, "mode_edit")
      );
    },

    async deleteUser() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await userProvider.deleteUser(
                this.$data.user.id
              );

              if (response.status === 200) {
                this.$data.user = new User();

                this.$data.user.logout();

                this.updateUser(this.$data.user);
                this.updateAuthToken("");
                this.updateUsersSelectedApp(new App());
                this.removeUsersApps();

                if (this.$router.currentRoute.path !== "/") {
                  this.$router.push("/");
                }

                showToast(
                  this,
                  ToastMethods["info"],
                  response.message,
                  defaultToastOptions()
                );
              } else {
                showToast(
                  this,
                  ToastMethods["error"],
                  response.message,
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
        "Are you sure you want to delete your account?",
        actionToastOptions(action, "delete")
      );
    },

    async refresh() {
      try {
        await this.reset();
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      }
    },

    async reset() {
      const response = await userProvider.getUser(this.$data.user.id);

      if (response.success) {
        this.$data.user = response.user;
        this.$data.user.login();
        this.updateUser(this.$data.user);
      } else {
        showToast(
          this,
          ToastMethods["error"],
          response.message,
          defaultToastOptions()
        );
      }
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser", "getAppId"]),
  },
  watch: {
    "$store.state.settingsModule.user": {
      handler: function (val, oldVal) {
        this.$data.user = new User(this.getUser);
      },
    },
  },
  created() {
    this.$data.user = new User(this.getUser);
  },
};
</script>
