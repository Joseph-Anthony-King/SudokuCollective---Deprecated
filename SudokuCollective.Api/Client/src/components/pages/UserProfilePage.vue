<template>
  <v-container fluid>
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
          <v-row>
            <v-col cols="12" lg="6" xl="6">
              <v-text-field
                v-model="user.id"
                label="Id"
                prepend-icon="mdi-account-circle"
                readonly
              ></v-text-field>
              <v-text-field
                v-model="user.userName"
                label="User Name"
                prepend-icon="mdi-account-circle"
                readonly
              ></v-text-field>
              <v-text-field
                v-model="user.firstName"
                label="First Name"
                prepend-icon="mdi-account-circle"
                readonly
              ></v-text-field>
              <v-text-field
                v-model="user.lastName"
                label="Last Name"
                prepend-icon="mdi-account-circle"
                readonly
              ></v-text-field>
              <v-text-field
                v-model="user.nickName"
                label="Nickname"
                prepend-icon="mdi-account-circle"
                readonly
              ></v-text-field>
              <v-checkbox
                v-model="user.isAdmin"
                label="Admin Privileges"
                readonly
              ></v-checkbox>
              <v-checkbox
                v-if="user.isSuperUser"
                v-model="user.isSuperUser"
                label="Super User Privileges"
                readonly
              ></v-checkbox>
            </v-col>
            <v-col cols="12" lg="6" xl="6">
              <v-text-field
                v-model="displayDateCreated"
                label="Date Created"
                hint="MM/DD/YYYY format"
                persistent-hint
                prepend-icon="mdi-calendar"
                readonly
              ></v-text-field>
              <v-text-field
                v-if="user.dateUpdated !== '0001-01-01T00:00:00Z'"
                v-model="displayDateUpdated"
                label="Date Updated"
                hint="MM/DD/YYYY format"
                persistent-hint
                prepend-icon="mdi-calendar"
                readonly
              ></v-text-field>
              <v-text-field
                v-model="user.email"
                label="Email"
                readonly
                prepend-icon="mdi-email"
              ></v-text-field>
              <v-checkbox
                v-model="user.emailConfirmed"
                label="Email Confirmed"
                readonly
              ></v-checkbox>
            </v-col>
          </v-row>
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
            <v-col v-if="!user.isAdmin">
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
                    @click="editingProfile = true"
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
          </v-row>
        </v-container>
      </v-card-actions>
    </v-card>

    <v-dialog v-model="editingProfile" persistent max-width="600px">
      <EditProfileForm
        :editProfileFormStatus="editingProfile"
        v-on:edit-user-profile-event="closeEditing"
      />
    </v-dialog>
  </v-container>
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
import { mapActions } from "vuex";
import { userService } from "@/services/userService/user.service";
import { registerService } from "@/services/registerService/register.service";
import { appService } from "@/services/appService/app.service";
import EditProfileForm from "@/components/forms/EditProfileForm";
import User from "@/models/user";
import PageListModel from "@/models/viewModels/pageListModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";
import {
  passwordReset,
  convertStringToDateTime,
} from "@/helpers/commonFunctions/commonFunctions";

export default {
  name: "UserProfilePage",
  components: {
    EditProfileForm,
  },
  data: () => ({
    user: new User(),
    editingProfile: false,
  }),
  methods: {
    ...mapActions("settingsModule", [
      "updateUser"
    ]),

    async resendEmailConfirmation() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await registerService.putResendEmailConfirmation(
                new PageListModel()
              );

              if (response.status === 200) {
                this.$data.user = new User(response.data.user);
                this.updateUser(this.$data.user);
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
              const response = await userService.putCancelEmailConfirmation(
                new PageListModel()
              );

              if (response.status === 200) {
                this.$data.user = new User(response.data.user);
                this.updateUser(this.$data.user);
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
              const response = await userService.putResendPasswordReset(
                new PageListModel()
              );

              if (response.status === 200) {
                this.$data.user = new User(response.data.user);
                this.updateUser(this.$data.user);
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
              const response = await userService.putCancelPasswordReset(
                new PageListModel()
              );

              if (response.status === 200) {
                this.$data.user = new User(response.data.user);
                this.updateUser(this.$data.user);
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
              const response = await userService.putCancelAllEmailRequests(
                new PageListModel()
              );

              if (response.status === 200) {
                this.$data.user = new User(response.data.user);
                this.updateUser(this.$data.user);
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
              const response = await appService.postObtainAdminPrivileges();

              if (response.status === 200) {
                this.$data.user = new User(response.data.user);
                this.updateUser(this.$data.user);
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
        "Are you sure you want to add admin privileges to your account?",
        actionToastOptions(action, "mode_edit")
      );
    },
    
    async refresh() {
      try {
        await this.reset();
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      }
    },

    closeEditing() {
      this.$data.user = this.$store.getters["settingsModule/getUser"];
      this.$data.editingProfile = false;
    },

    async reset() {
      let user = await userService.getUser(
        this.$data.user.id,
        false
      );
      
      this.$data.user = new User(user);
      this.updateUser(this.$data.user);
    },
  },
  computed: {
    displayDateCreated: function () {
      return convertStringToDateTime(this.$data.user.dateCreated);
    },
    displayDateUpdated: function () {
      return convertStringToDateTime(this.$data.user.dateUpdated);
    },
  },
  watch: {
    "$store.state.settingsModule.user": function () {
      this.$data.user = new User(this.$store.getters["settingsModule/getUser"]);
    },
  },
  created() {    
    this.$data.user = new User(this.$store.getters["settingsModule/getUser"]);
  },
};
</script>
