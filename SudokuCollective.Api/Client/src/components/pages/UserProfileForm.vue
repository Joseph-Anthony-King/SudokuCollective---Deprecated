<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h1 style="text-align: center" v-if="user.isActive">
          Your Account is Active
        </h1>
        <h1
          style="text-align: center"
          v-if="!user.isActive"
          class="account-status-warning"
        >
          Your Account is Deativated, Please Contact the Administrator
        </h1>
      </v-col>
    </v-row>
    <v-row style="min-height: 25px"></v-row>
    <v-row>
      <v-col cols="6">
        <v-row>
          <label class="label">User Name:</label>
          <span class="label-spacer"></span>
          <p class="userInfo">{{ user.userName }}</p>
        </v-row>
        <v-row>
          <label class="label">First Name:</label>
          <span class="label-spacer"></span>
          <p class="userInfo">{{ user.firstName }}</p>
        </v-row>
        <v-row>
          <label class="label">Last Name:</label>
          <span class="label-spacer"></span>
          <p class="userInfo">{{ user.lastName }}</p>
        </v-row>
        <v-row>
          <label class="label">Nickname:</label>
          <span class="label-spacer"></span>
          <p class="userInfo">{{ user.nickName }}</p>
        </v-row>
        <v-row>
          <label class="label">Admin Privileges:</label>
          <span class="label-spacer"></span>
          <v-icon v-if="user.isAdmin" color="green">mdi-check</v-icon>
          <v-icon v-if="!user.isAdmin" color="red">mdi-close</v-icon>
        </v-row>
      </v-col>
      <v-col cols="6">
        <v-row>
          <label class="label">Email:</label>
          <span class="label-spacer"></span>
          <p class="userInfo">{{ user.email }}</p>
        </v-row>
        <v-row>
          <label class="label">Email Confirmed:</label>
          <span class="label-spacer"></span>
          <v-icon v-if="user.emailConfirmed" color="green">mdi-check</v-icon>
          <v-icon v-if="!user.emailConfirmed" color="red">mdi-close</v-icon>
        </v-row>
        <v-row style="min-height: 25px"></v-row>
        <v-row>
          <label class="label">Date Created:</label>
          <span class="label-spacer"></span>
          <p class="userInfo">
            {{ new Date(user.dateCreated).toLocaleString() }}
          </p>
        </v-row>
        <v-row v-if="user.dateUpdated !== '0001-01-01T00:00:00Z'">
          <label class="label">Date Updated:</label>
          <span class="label-spacer"></span>
          <p class="userInfo">
            {{ new Date(user.dateUpdated).toLocaleString() }}
          </p>
        </v-row>
      </v-col>
    </v-row>
    <v-row>
      <v-col col="12">
        <h2
          class="account-status"
          v-if="
            !user.receivedRequestToUpdateEmail &&
            !user.receivedRequestToUpdatePassword &&
            user.emailConfirmed
          "
        >
          No Outstanding Requests for this Account
        </h2>
        <h2 class="account-status-warning" v-if="!user.emailConfirmed">
          Please Confirm Your Email
        </h2>
        <h2
          class="account-status-warning"
          v-if="user.receivedRequestToUpdateEmail"
        >
          Received Request to Update Email
        </h2>
        <h2
          class="account-status-warning"
          v-if="user.receivedRequestToUpdatePassword"
        >
          Received Request to Update Password
        </h2>
      </v-col>
    </v-row>
    <div style="padding: 50px 0px 0px 0px"></div>
    <hr />
    <v-row>
      <v-col col="12">
        <p class="available-actions">Available Actions</p>
      </v-col>
    </v-row>
    <v-row style="text-align: center">
      <v-col cols="12">
        <v-btn color="blue darken-1" text @click="refreshProfile">
          Refresh Profile
        </v-btn>
        <v-btn color="blue darken-1" text @click="editingProfile = true">
          Edit Profile
        </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="cancelEmailConfirmation"
          v-if="user.receivedRequestToUpdateEmail || (!user.emailConfirmed && user.dateUpdated !== '0001-01-01T00:00:00Z')"
        >
          Cancel Email Confirmation
        </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="resetPassword"
          v-if="!user.receivedRequestToUpdatePassword"
        >
          Reset Password
        </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="cancelResetPassword"
          v-if="user.receivedRequestToUpdatePassword"
        >
          Cancel Reset Password
        </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="cancelAllEmailRequests"
          v-if="(user.receivedRequestToUpdateEmail || (!user.emailConfirmed && user.dateUpdated !== '0001-01-01T00:00:00Z')) && user.receivedRequestToUpdatePassword"
        >
          Cancel All Email Requests
        </v-btn>
      </v-col>
    </v-row>

    <v-dialog v-model="editingProfile" persistent max-width="600px">
      <EditProfileForm
        :editProfileFormStatus="editingProfile"
        v-on:edit-user-profile-event="closeEditing"
      />
    </v-dialog>
  </v-container>
</template>

<style scoped>
.label {
  font-weight: bold;
  font-size: x-large;
}
.userInfo {
  font-size: x-large;
}
.label-spacer {
  min-width: 10px;
}
.account-status {
  text-align: center;
}
.available-actions {
  padding: 25px 0 0 0;
  text-align: center;
}
.account-status-warning {
  text-align: center;
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
import { passwordReset } from "@/helpers/commonFunctions/commonFunctions";

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
              const response = await userService.putCancelPasswordReset(new PageListModel());

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
              const response = await userService.putCancelEmailConfirmation(new PageListModel());

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
              const response = await userService.putCancelAllEmailRequests(new PageListModel());

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
  mounted() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
  },
};
</script>