<template>
  <v-card>
    <v-card-title>
      <span class="headline">Create App</span>
    </v-card-title>
    <v-form v-model="createAppFormIsValid" ref="createAppForm">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="name"
                label="Name"
                prepend-icon="mdi-account-edit"
                :rules="stringRequiredRules"
                required
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="devUrl"
                label="Development Url (Not Required)"
                prepend-icon="mdi-account-edit"
                :rules="urlRules"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="liveUrl"
                label="Live Url (Not Required)"
                prepend-icon="mdi-account-edit"
                :rules="urlRules"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
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
          <span>Reset the create app form</span>
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
          <span>Close the create app form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="submit"
              :disabled="!dirty || !createAppFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Submit
            </v-btn>
          </template>
          <span>Submit the create app form</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
/* eslint-disable no-useless-escape */
import { mapActions } from "vuex";
import { appService } from "@/services/appService/appService";
import CreateAppModel from "@/models/viewModels/createAppModel";
import App from "@/models/app";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

export default {
  name: "CreateAppForm",
  data: () => ({
    name: "",
    devUrl: "",
    liveUrl: "",
    createAppFormIsValid: true,
    dirty: false,
  }),
  methods: {
    ...mapActions("appModule", ["updateApps"]),

    submit() {
      if (this.getCreateAppFormIsValid) {
        const action = [
          {
            text: "Yes",
            onClick: async (e, toastObject) => {
              toastObject.goAway(0);

              try {
                const response = await appService.postLicense(
                  new CreateAppModel(
                    this.$data.name,
                    this.$data.devUrl,
                    this.$data.liveUrl
                  )
                );

                if (response.status === 201) {
                  const appsResponse = await appService.getMyApps();

                  if (appsResponse.data.success) {
                    let tempArray = [];

                    for (const app of appsResponse.data.apps) {
                      const myApp = new App(app);
                      const licenseResponse = await appService.getLicense(
                        myApp.id
                      );
                      if (licenseResponse.data.success) {
                        myApp.updateLicense(licenseResponse.data.license);
                      }
                      const appUsersResponse = await appService.getAppUsers(myApp.id);
                      appUsersResponse.data.users.forEach((user) => {
                        const tempUser = new User(user);
                        myApp.users.push(tempUser);
                      });
                      tempArray.push(myApp);
                    }

                    this.updateApps(tempArray);
                  }

                  this.reset();

                  this.$emit("app-created-event", response.data.app.id, null);
                } else if (response.status === 404) {
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
                    response.data.message,
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
          "Are you sure you want to create this app?",
          actionToastOptions(action, "mode_edit")
        );
      }
    },

    reset() {
      this.$refs.createAppForm.reset();
      document.activeElement.blur();
    },

    close() {
      this.$emit("create-form-closed-event", null, null);
      this.reset();
    },
  },
  computed: {
    stringRequiredRules() {
      return [(v) => !!v || "Value is required"];
    },

    urlRules() {
      const regex = /https?:[0-9]*\/\/[\w!?/\+\-_~=;\.,*&@#$%\(\)\'\[\]]+/;
      return [(v) => !v || regex.test(v) || "Must be a valid url"];
    },

    resetCreateAppFormIsValid() {
      return !this.createAppFormIsValid;
    },

    getCreateAppFormIsValid() {
      return this.createAppFormIsValid;
    },
  },
  mounted() {
    let self = this;
    window.addEventListener("keyup", function (event) {
      if (
        event.key === "Enter" &&
        self.$data.createAppFormIsValid &&
        self.$data.dirty
      ) {
        self.submit();
      }
    });
  },
};
</script>
