<template>
  <v-card>
    <v-card-text>
      <v-container fluid>
        <v-card-title
          class="justify-center"
          v-if="
            app.isActive === false ||
            (app.devUrl === '' && app.inDevelopment) ||
            (app.liveUrl === '' && !app.inDevelopment)
          "
          >{{ app.name }}</v-card-title
        >
        <v-card-title
          class="justify-center"
          v-if="
            app.isActive &&
            ((app.devUrl !== '' && app.inDevelopment) ||
              (app.liveUrl !== '' && !app.inDevelopment))
          "
        >
          <a
            :href="app.inDevelopment ? app.devUrl : app.liveUrl"
            target="blank"
            class="app-card-title"
          >
            {{ app.name }}
          </a>
        </v-card-title>
        <hr class="title-spacer" />
        <AppProfileWidget :app="app" />
      </v-container>
    </v-card-text>
    <hr />
    <v-card-title class="justify-center">Available Actions</v-card-title>
    <v-card-actions>
      <v-container>
        <v-row dense>
          <v-col>
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="blue darken-1"
                  text
                  @click="refreshApp"
                  v-bind="attrs"
                  v-on="on"
                >
                  Refresh App
                </v-btn>
              </template>
              <span>Get your latest app data from the api</span>
            </v-tooltip>
          </v-col>
          <v-col>
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="blue darken-1"
                  text
                  @click="openEditAppForm"
                  v-bind="attrs"
                  v-on="on"
                >
                  Edit App
                </v-btn>
              </template>
              <span>Edit your app</span>
            </v-tooltip>
          </v-col>
          <v-col>
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="blue darken-1"
                  text
                  @click="resetApp"
                  v-bind="attrs"
                  v-on="on"
                >
                  Reset App
                </v-btn>
              </template>
              <span>Reset your app</span>
            </v-tooltip>
          </v-col>
          <v-col>
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn
                  class="button-full"
                  color="red darken-1"
                  text
                  @click="deleteApp"
                  v-bind="attrs"
                  v-on="on"
                >
                  Delete App
                </v-btn>
              </template>
              <span>Delete your app</span>
            </v-tooltip>
          </v-col>
        </v-row>
      </v-container>
    </v-card-actions>
  </v-card>
</template>

<style scoped>
.close-hover:hover {
  cursor: pointer;
}
</style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { appProvider } from "@/providers/appProvider";
import User from "@/models/user";
import App from "@/models/app";
import AppProfileWidget from "@/components/widgets/apps/AppProfileWidget";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

export default {
  name: "AppInfoWidget",
  components: {
    AppProfileWidget,
  },
  data: () => ({
    user: new User(),
    app: new App(),
  }),
  methods: {
    ...mapActions("appModule", [
      "updateUsersSelectedApp",
      "updateUsersApps",
      "removeUsersApp",
      "replaceUsersApp",
      "replaceApp",
    ]),

    async copyLicenseToClipboard() {
      try {
        await navigator.clipboard.writeText(this.$data.app.license);
        showToast(
          this,
          ToastMethods["success"],
          "Copied license to clipboard",
          defaultToastOptions()
        );
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      }
    },

    async resetApp() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await appProvider.resetApp(this.$data.app);

              if (response.status === 200) {
                this.$data.app = response.app;

                this.updateUsersApps(response.apps);
                this.updateUsersSelectedApp(this.$data.app);

                if (this.$data.user.isSuperUser) {
                  this.replaceApp(this.$data.app);
                }

                showToast(
                  this,
                  ToastMethods["success"],
                  response.message,
                  defaultToastOptions()
                );
              } else if (response.status === 404) {
                showToast(
                  this,
                  ToastMethods["error"],
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
        "Are you sure you want to clear all games and reset this app?",
        actionToastOptions(action, "clear")
      );
    },

    async deleteApp() {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await appProvider.deleteApp(this.$data.app);

              if (response.status === 200) {
                this.removeUsersApp(this.$data.app);
                this.updateUsersApps(response.apps);
                this.updateUsersSelectedApp(response.app);

                showToast(
                  this,
                  ToastMethods["success"],
                  response.message,
                  defaultToastOptions()
                );

                this.$emit("close-app-widget-event", null, null);
              } else if (response.status === 404) {
                showToast(
                  this,
                  ToastMethods["error"],
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
        "Are you sure you want to delete this app?",
        actionToastOptions(action, "delete")
      );
    },

    async refreshApp() {
      var response = await appProvider.getApp(this.$data.app.id);

      if (response.isSuccess) {
        this.$data.app = new App(response.app);
        this.updateUsersSelectedApp(this.$data.app);
        this.replaceUsersApp(this.$data.app);

        showToast(
          this,
          ToastMethods["success"],
          "App Refreshed",
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
    },

    openEditAppForm() {
      this.$emit("open-edit-app-form-event", null, null);
    },

    close() {
      this.$emit("close-app-widget-event", null, null);
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getUsersSelectedApp"]),

    getAccessPeriod() {
      let duration;

      switch (this.$data.app.accessDuration) {
        case 1:
          duration = "one";
          break;

        case 2:
          duration = "two";
          break;

        case 3:
          duration = "three";
          break;

        case 4:
          duration = "four";
          break;

        case 5:
          duration = "five";
          break;

        case 6:
          duration = "six";
          break;

        case 7:
          duration = "seven";
          break;

        case 8:
          duration = "eight";
          break;

        case 9:
          duration = "nine";
          break;

        case 10:
          duration = "ten";
          break;

        case 11:
          duration = "eleven";
          break;

        case 12:
          duration = "twelve";
          break;

        case 13:
          duration = "thirteen";
          break;

        case 14:
          duration = "fourteen";
          break;

        case 15:
          duration = "fifteen";
          break;

        case 16:
          duration = "sixteen";
          break;

        case 17:
          duration = "seventeen";
          break;

        case 18:
          duration = "eighteen";
          break;

        case 19:
          duration = "nineteen";
          break;

        case 20:
          duration = "twenty";
          break;

        case 21:
          duration = "twenty-one";
          break;

        case 22:
          duration = "twenty-two";
          break;

        case 23:
          duration = "twenty-three";
          break;

        case 24:
          duration = "twenty-four";
          break;

        case 25:
          duration = "twenty-five";
          break;

        case 26:
          duration = "twenty-six";
          break;

        case 27:
          duration = "twenty-seven";
          break;

        case 28:
          duration = "twenty-eight";
          break;

        case 29:
          duration = "twenty-nine";
          break;

        case 30:
          duration = "thirty";
          break;

        case 31:
          duration = "thirty-one";
          break;

        case 32:
          duration = "thirty-two";
          break;

        case 33:
          duration = "thirty-three";
          break;

        case 34:
          duration = "thirty-four";
          break;

        case 35:
          duration = "thirty-five";
          break;

        case 36:
          duration = "thirty-six";
          break;

        case 37:
          duration = "thirty-seven";
          break;

        case 38:
          duration = "thirty-eight";
          break;

        case 39:
          duration = "thirty-nine";
          break;

        case 40:
          duration = "fourty";
          break;

        case 41:
          duration = "fourty-one";
          break;

        case 42:
          duration = "fourty-two";
          break;

        case 43:
          duration = "fourty-three";
          break;

        case 44:
          duration = "fourty-four";
          break;

        case 45:
          duration = "fourty-five";
          break;

        case 46:
          duration = "fourty-six";
          break;

        case 47:
          duration = "fourty-seven";
          break;

        case 48:
          duration = "fourty-eight";
          break;

        case 49:
          duration = "fourty-nine";
          break;

        case 50:
          duration = "fifty";
          break;

        case 51:
          duration = "fifty-one";
          break;

        case 52:
          duration = "fifty-two";
          break;

        case 53:
          duration = "fifty-three";
          break;

        case 54:
          duration = "fifty-four";
          break;

        case 55:
          duration = "fifty-five";
          break;

        case 56:
          duration = "fifty-six";
          break;

        case 57:
          duration = "fifty-seven";
          break;

        case 58:
          duration = "fifty-eight";
          break;

        default:
          duration = "fifty-nine";
      }

      let period;

      switch (this.$data.app.timeFrame) {
        case 1:
          period = this.$data.app.accessDuration === 1 ? "second" : "seconds";
          break;

        case 2:
          period = this.$data.app.accessDuration === 1 ? "minute" : "minutes";
          break;

        case 3:
          period = this.$data.app.accessDuration === 1 ? "hour" : "hours";
          break;

        case 4:
          period = this.$data.app.accessDuration === 1 ? "day" : "days";
          break;

        default:
          period = this.$data.app.accessDuration === 1 ? "month" : "months";
      }

      return `Good for ${duration} ${period}`;
    },

    isOwnersisEmailConfirmed() {
      const owner = this.$data.app.users.find(
        (user) => user.id === this.$data.app.ownerId
      );
      return owner.isEmailConfirmed;
    },
  },
  watch: {
    "$store.state.appModule.usersSelectedApp": {
      handler: function (val, oldVal) {
        this.$data.app = this.getUsersSelectedApp;
      },
    },
  },
  mounted() {
    this.$data.app = this.getUsersSelectedApp;
    this.$data.user = this.getUser;
  },
};
</script>
