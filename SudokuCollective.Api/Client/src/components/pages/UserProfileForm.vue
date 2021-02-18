<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h1 style="text-align: center">User Profile</h1>
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
        <v-row v-if="user.dateUpdated !== '0001-01-01T00:00:00'">
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
            !user.receivedRequestToUpdatePassword
          "
        >
          No Outstanding Requests for this Account
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
    <v-row>
      <v-btn color="blue darken-1" text @click="editingProfile = true">
        Edit Profile
      </v-btn>
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
.account-status-warning {
  text-align: center;
  color: red;
}
</style>

<script>
import EditProfileForm from "@/components/forms/EditProfileForm";
import User from "@/models/user";

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