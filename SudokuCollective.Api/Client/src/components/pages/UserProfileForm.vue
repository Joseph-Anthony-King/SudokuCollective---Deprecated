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
    <div class="user-card-spacer"></div>
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
            <v-col>
              <v-btn
                class="button-full"
                color="blue darken-1"
                text
                @click="refreshProfile"
              >
                Refresh Profile
              </v-btn>
            </v-col>
            <v-col>
              <v-btn
                class="button-full"
                color="blue darken-1"
                text
                @click="editingProfile = true"
              >
                Edit Profile
              </v-btn>
            </v-col>
            <v-col
              class="button-full"
              v-if="
                user.receivedRequestToUpdateEmail ||
                (!user.emailConfirmed &&
                  user.dateUpdated !== '0001-01-01T00:00:00Z')
              "
            >
              <v-btn
                class="button-full"
                color="blue darken-1"
                text
                @click="cancelEmailConfirmation"
              >
                Cancel Email Confirmation
              </v-btn>
            </v-col>
            <v-col
              v-if="
                !user.receivedRequestToUpdatePassword && user.emailConfirmed
              "
            >
              <v-btn
                class="button-full"
                color="blue darken-1"
                text
                @click="resetPassword"
              >
                Reset Password
              </v-btn>
            </v-col>
            <v-col v-if="user.receivedRequestToUpdatePassword">
              <v-btn
                class="button-full"
                color="blue darken-1"
                text
                @click="cancelResetPassword"
              >
                Cancel Reset Password
              </v-btn>
            </v-col>
            <v-col
              v-if="
                (user.receivedRequestToUpdateEmail ||
                  (!user.emailConfirmed &&
                    user.dateUpdated !== '0001-01-01T00:00:00Z')) &&
                user.receivedRequestToUpdatePassword
              "
            >
              <v-btn color="blue darken-1" text @click="cancelAllEmailRequests">
                Cancel All Email Requests
              </v-btn>
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
.title-spacer {
  margin: 0 0 20px 0;
}
.user-card-spacer {
  min-height: 30px;
}
.success {
  color: white;
}
.warning {
  color: red;
}
</style>

<script>
import { userService } from "@/services/userService/user.service";
import EditProfileForm from "@/components/forms/EditProfileForm";
import store from "../../store";
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
  name: "UserProfileForm",
  components: {
    EditProfileForm,
  },
  data: () => ({
    user: {},
    editingProfile: false,
  }),
  methods: {
    async resetPassword() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              let result = await passwordReset(this.$data.user.email, this);

              if (result) {
                this.refresh();
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
                store.dispatch("userModule/updateUser", response.data.user);
                this.$data.user.shallowClone(response.data.user);
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
                store.dispatch("userModule/updateUser", response.data.user);
                this.$data.user.shallowClone(response.data.user);
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
        actionToastOptions(action, "mode_edit")
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
                store.dispatch("userModule/updateUser", response.data.user);
                this.$data.user.shallowClone(response.data.user);
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
    refreshProfile() {
      try {
        this.refresh();
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      }
    },
    async refresh() {
      let user = await userService.getUser(
        this.$data.user.id,
        new PageListModel(),
        false
      );
      store.dispatch("userModule/updateUser", user);
      this.$data.user.shallowClone(user);
    },
    closeEditing() {
      this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
      this.$data.editingProfile = false;
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
  mounted() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
  },
};
</script>