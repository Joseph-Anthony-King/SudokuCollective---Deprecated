<template>
  <v-card>
    <v-card-title>
      <span class="headline">Edit App</span>
    </v-card-title>
    <v-form v-model="editAppFormIsValid" ref="editAppForm">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12" lg="6" xl="6">
              <v-text-field
                label="Name"
                v-model="app.name"
                prepend-icon="mode_edit"
                :rules="stringRequiredRules"
                required
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
              <v-select
                v-model="app.accessDuration"
                prepend-icon="av_timer"
                :items="durations"
                item-text="durations"
                item-value="durations"
                label="Duration for authorization token before it expires"
                dense
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-select>
              <v-select
                v-model="app.timeFrame"
                prepend-icon="av_timer"
                :items="timeFrames"
                item-text="label"
                item-value="value"
                label="Time frame for authorization token before it expires"
                dense
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-select>
              <v-text-field
                label="Development Url"
                v-model="app.devUrl"
                prepend-icon="mode_edit"
                :rules="urlRules"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
              <v-text-field
                label="Production Url"
                v-model="app.liveUrl"
                prepend-icon="mode_edit"
                :rules="urlRules"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
              <v-text-field
                label="Custom Email Confirmation Action"
                v-model="app.customEmailConfirmationAction"
                prepend-icon="mode_edit"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
              <v-text-field
                v-model="app.customPasswordResetAction"
                label="Custom Password Reset Action"
                prepend-icon="mode_edit"
                :rules="urlRules"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12" lg="6" xl="6">
              <v-checkbox
                v-model="app.isActive"
                :label="
                  app.isActive
                    ? 'App is Active'
                    : 'App is deactivated, API requests will be denied as invalid'
                "
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-checkbox>
              <v-checkbox
                v-model="app.inDevelopment"
                :label="
                  app.inDevelopment
                    ? 'App is in Development'
                    : 'App is in Production'
                "
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-checkbox>
              <v-checkbox
                v-model="app.permitSuperUserAccess"
                :label="
                  app.permitSuperUserAccess
                    ? 'Super User has Admin Access Rights to this App'
                    : 'Super User does not have Admin Access Rights to this App'
                "
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-checkbox>
              <v-checkbox
                v-model="app.permitCollectiveLogins"
                :label="
                  app.permitCollectiveLogins
                    ? 'User Registration is not Required to Gain Access to this App'
                    : 'User Registration is Required to Gain Access to this App'
                "
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-checkbox>
              <v-checkbox
                v-model="app.disableCustomUrls"
                :label="
                  app.disableCustomUrls
                    ? 'Custom Urls for Email Confirmations and Password Resets are disabled'
                    : 'Custom Urls for Email Confirmations and Password Resets are enabled'
                "
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-checkbox>
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
          <span>Reset the edit app form</span>
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
          <span>Close the edit app form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="submit"
              :disabled="!dirty || !editAppFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Submit
            </v-btn>
          </template>
          <span>Submit the edit app form</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
/* eslint-disable no-useless-escape, no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { appProvider } from "@/providers/appProvider";
import App from "@/models/app";
import UpdateAppModel from "@/models/viewModels/updateAppModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

export default {
  name: "EditAppForm",
  props: ["editAppFormStatus"],
  data: () => ({
    app: new App(),
    editAppFormIsValid: true,
    dirty: false,
    submitInvoked: false,
    timeFrames: [],
    durations: [],
  }),
  methods: {
    ...mapActions("appModule", ["updateSelectedApp", "replaceApp"]),

    submit() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);
            this.$data.submitInvoked = false;

            try {
              const data = new UpdateAppModel(
                this.$data.app.id,
                this.$data.app.name,
                this.$data.app.devUrl,
                this.$data.app.liveUrl,
                this.$data.app.isActive,
                this.$data.app.inDevelopment,
                this.$data.app.permitSuperUserAccess,
                this.$data.app.permitCollectiveLogins,
                this.$data.app.disableCustomUrls,
                this.$data.app.customEmailConfirmationAction,
                this.$data.app.customPasswordResetAction,
                this.$data.app.timeFrame,
                this.$data.app.accessDuration
              );

              const response = await appProvider.updateApp(data);

              if (response.status === 200) {
                this.resetEditProfileFormStatus;

                this.$data.app = response.app;

                this.updateSelectedApp(this.$data.app);

                this.replaceApp(this.$data.app);

                showToast(
                  this,
                  ToastMethods["success"],
                  "Your app has been updated",
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
        "Are you sure you want to update this app?",
        actionToastOptions(action, "mode_edit")
      );
    },

    reset() {
      this.$data.app = new App(this.getSelectedApp);
      this.$data.editAppFormIsValid = true;
      this.$data.dirty = false;
      document.activeElement.blur();
    },

    close() {
      this.$emit("edit-app-closed-event", null, null);
      this.reset();
    },
  },
  computed: {
    ...mapGetters("appModule", ["getSelectedApp"]),

    stringRequiredRules() {
      return [(v) => !!v || "Value is required"];
    },

    urlRules() {
      const regex = /https?:[0-9]*\/\/[\w!?/\+\-_~=;\.,*&@#$%\(\)\'\[\]]+/;
      return [(v) => !v || regex.test(v) || "Must be a valid url"];
    },

    resetEditAppFormStatus() {
      return !this.editAppFormStatus;
    },

    getEditAppFormStatus() {
      return this.editAppFormStatus;
    },
  },
  watch: {
    "$store.state.appModule.selectedApp": {
      handler: function (val, oldVal) {
        this.$data.app = new App(this.getSelectedApp);
      },
    },
    "app.timeFrame": {
      handler: function (val, oldVal) {
        if (val === 1) {
          this.$data.durations = [];
          for (let i = 1; i <= 59; i++) {
            this.$data.durations.push(i);
          }
        } else if (val === 2) {
          this.$data.durations = [];
          for (let i = 1; i <= 59; i++) {
            this.$data.durations.push(i);
          }
        } else if (val === 3) {
          this.$data.durations = [];
          for (let i = 1; i <= 23; i++) {
            this.$data.durations.push(i);
          }
          if (this.app.accessDuration > 23) {
            this.app.accessDuration = 23;
          }
        } else if (val === 4) {
          this.$data.durations = [];
          for (let i = 1; i <= 31; i++) {
            this.$data.durations.push(i);
          }
          if (this.app.accessDuration > 31) {
            this.app.accessDuration = 31;
          }
        } else {
          this.$data.durations = [];
          for (let i = 1; i <= 12; i++) {
            this.$data.durations.push(i);
          }
          if (this.app.accessDuration > 12) {
            this.app.accessDuration = 12;
          }
        }
      },
    },
  },
  async created() {
    this.$data.app = new App(this.getSelectedApp);

    try {
      const response = await appProvider.getTimeFrames();

      this.$data.timeFrames = response;
    } catch (error) {
      console.log(error);
    }
  },
  mounted() {
    if (this.$props.editAppFormStatus) {
      let self = this;
      window.addEventListener("keyup", function (event) {
        if (event.key === "Enter" && self.$data.dirty) {
          self.submit();
        }
      });
    }
  },
};
</script>
