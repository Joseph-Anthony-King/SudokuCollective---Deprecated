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
                :rules="stringRequiredRules"
                required
                @click="!dirty ? (dirty = true) : null"
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
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="user.nickName"
                label="Nickname (Not Required)"
                prepend-icon="mdi-account-edit"
                required
                @click="!dirty ? (dirty = true) : null"
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
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="resetForm"> Reset </v-btn>
        <v-btn color="blue darken-1" text @click="close"> Close </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="submit"
          :disabled="!dirty || !editProfileFormIsValid"
        >
          Submit
        </v-btn>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
import { userService } from "@/services/userService/user.service";
import User from "@/models/user";
import PageListModel from "@/models/viewModels/pageListModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "EditProfileForm",
  props: ["editProfileFormStatus"],
  data: () => ({
    editProfileFormIsValid: true,
    user: {},
    dirty: false,
  }),
  methods: {
    resetForm() {
      this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
      this.$data.dirty = false;
    },

    async submit() {
      try {
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

          this.close();
        }
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      }
    },

    close() {
      this.$emit("edit-user-profile-event", null, null);
      this.resetForm();
    },
  },
  computed: {
    stringRequiredRules() {
      return [(v) => !!v || "Value is required"];
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

    resetEditProfileFormStatus() {
      return !this.editProfileFormStatus;
    },

    getEditProfileFormStatus() {
      return this.editProfileFormStatus;
    },
  },
  mounted() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.$store.getters["userModule/getUser"]);
  },
};
</script>