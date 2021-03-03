<template>
  <v-card>
    <v-card-title>
      <span class="headline">Edit Profile</span>
    </v-card-title>
    <v-form v-model="editProfileFormIsValid" ref="editProfileForm">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="user.userName"
                label="User Name"
                prepend-icon="mdi-account-edit"
                :rules="userNameRules"
                required
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="user.firstName"
                label="First Name"
                prepend-icon="mdi-account-edit"
                :rules="stringRequiredRules"
                required
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="user.lastName"
                label="Last Name"
                prepend-icon="mdi-account-edit"
                :rules="stringRequiredRules"
                required
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="user.nickName"
                label="Nickname (Not Required)"
                prepend-icon="mdi-account-edit"
                required
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="user.email"
                label="Email"
                prepend-icon="mdi-email"
                required
                :rules="emailRules"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
                v-if="user.emailConfirmed"
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
          <span>Reset the edit profile form</span>
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
              close
            </v-btn>
          </template>
          <span>Close the edit profile form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="submit"
              :disabled="!dirty || !editProfileFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Submit
            </v-btn>
          </template>
          <span>Submit the edit profile form</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { userService } from "@/services/userService/user.service";
import User from "@/models/user";
import PageListModel from "@/models/viewModels/pageListModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

export default {
  name: "EditProfileForm",
  props: ["editProfileFormStatus"],
  data: () => ({
    user: new User(),
    invalidUserNames: [],
    invalidEmails: [],
    editProfileFormIsValid: true,
    dirty: false,
    submitInvoked: false
  }),
  methods: {
    ...mapActions("settingsModule", ["updateUser"]),

    async submit() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);
            this.$data.submitInvoked = false;

            try {
              let userProfie = this.getUser;
              let updatingEmail = false;
              let oldEmail = "";

              if (userProfie.email !== this.$data.user.email) {
                updatingEmail = true;
                oldEmail = userProfie.email;
              }

              const response = await userService.updateUser(
                this.$data.user.id,
                this.$data.user.userName,
                this.$data.user.firstName,
                this.$data.user.lastName,
                this.$data.user.nickName,
                this.$data.user.email,
                new PageListModel()
              );

              if (response.status === 200) {
                this.resetEditProfileFormStatus;

                let user = await userService.getUser(
                  this.$data.user.id,
                  false
                );
                
                this.$data.user = new User(user);
                this.$data.user.login();
                this.updateUser(this.$data.user);

                if (updatingEmail) {
                  showToast(
                    this,
                    ToastMethods["success"],
                    `Your profile has been updated, please review ${oldEmail} to update your email`,
                    defaultToastOptions()
                  );
                } else {
                  showToast(
                    this,
                    ToastMethods["success"],
                    "Your profile has been updated",
                    defaultToastOptions()
                  );
                }
                this.close();
              } else if (response.status === 404) {
                if (
                  response.data.message ===
                    "Status Code 404: User Name Accepts Alphanumeric And Special Characters Except Double And Single Quotes" ||
                  response.data.message ===
                    "Status Code 404: User Name Not Unique" ||
                  response.data.message ===
                    "Status Code 404: User Name Required"
                ) {
                  this.$data.invalidUserNames.push(this.$data.user.userName);
                  this.$refs.editProfileForm.validate();
                  showToast(
                    this,
                    ToastMethods["error"],
                    response.data.message.substring(17),
                    defaultToastOptions()
                  );
                } else if (
                  response.data.message ===
                    "Status Code 404: Email Not Unique" ||
                  response.data.message === "Status Code 404: Email Required"
                ) {
                  this.$data.invalidEmails.push(this.$data.user.email);
                  this.$refs.editProfileForm.validate();
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
            this.$data.submitInvoked = false;
          },
        },
      ];

      showToast(
        this,
        ToastMethods["show"],
        "Are you sure you want to update your profile?",
        actionToastOptions(action, "mode_edit")
      );
    },

    reset() {
      this.$data.user = new User(this.getUser);
      this.$data.invalidUserNames = [];
      this.$data.invalidEmails = [];
      this.$data.editProfileFormIsValid = true;
      this.$data.dirty = false;
      this.$data.submitInvoked = false;
      document.activeElement.blur();
    },

    close() {
      this.$emit("edit-user-profile-event", null, null);
      this.reset();
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),

    userNameRules() {
      return [
        (v) => !!v || "User Name is required",
        (v) =>
          !this.$data.invalidUserNames.includes(v) || "User Name Not Unique",
      ];
    },

    stringRequiredRules() {
      return [(v) => !!v || "Value is required"];
    },

    emailRules() {
      const regex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return [
        (v) => !!v || "Email is required",
        (v) => !v || regex.test(v) || "Email must be valid",
        (v) => !this.$data.invalidEmails.includes(v) || "Email Not Unique",
      ];
    },

    resetEditProfileFormStatus() {
      return !this.editProfileFormStatus;
    },

    getEditProfileFormStatus() {
      return this.editProfileFormStatus;
    },
  },
  watch: {
    "$store.state.settingsModule.user": function () {
      this.$data.user = new User(this.getUser);
    },
  },
  created() {
    this.$data.user = new User(this.getUser);
  },
  mounted() {
    if (this.$props.editProfileFormStatus) {
      let self = this;
      window.addEventListener("keyup", function (event) {
        if (event.key === "Enter"
          && self.$data.dirty) {
          self.submit();
        }
      });
    }
  },
};
</script>
