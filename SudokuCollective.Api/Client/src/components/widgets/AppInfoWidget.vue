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
        <v-row>
          <v-col cols="12" lg="6" xl="6">
            <v-text-field
              v-model="app.id"
              label="Id"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.name"
              label="Name"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.license"
              label="App License"
              prepend-icon="wysiwyg"
              append-icon="content_copy"
              @click:append="copyLicenseToClipboard"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="getAccessPeriod"
              label="Authorization Token Access Period"
              prepend-icon="av_timer"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.devUrl"
              label="Development Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.liveUrl"
              label="Production Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.customEmailConfirmationAction"
              label="Custom Email Confirmation Action"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.customPasswordResetAction"
              label="Custom Password Reset Action"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.userCount"
              label="User Count"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.gameCount"
              v-if="app.id !== 1"
              label="Game Count"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
          </v-col>
          <v-col cols="12" lg="6" xl="6">
            <v-text-field
              v-model="getDateCreated"
              label="Date Created"
              hint="MM/DD/YYYY format"
              persistent-hint
              prepend-icon="mdi-calendar"
              readonly
            ></v-text-field>
            <v-text-field
              v-if="app.dateUpdated !== '0001-01-01T00:00:00Z'"
              v-model="getDateUpdated"
              label="Date Updated"
              hint="MM/DD/YYYY format"
              persistent-hint
              prepend-icon="mdi-calendar"
              readonly
            ></v-text-field>
            <v-checkbox
              v-model="app.isActive"
              :label="
                app.isActive
                  ? 'App is Active'
                  : 'App is deactivated, API requests will be denied as invalid'
              "
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="app.inDevelopment"
              :label="
                app.inDevelopment
                  ? 'App is in Development'
                  : 'App is in Production'
              "
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="app.permitSuperUserAccess"
              :label="
                app.permitSuperUserAccess
                  ? 'Super User has Admin Access Rights to this App'
                  : 'Super User does not have Admin Access Rights to this App'
              "
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="app.permitCollectiveLogins"
              :label="
                app.permitCollectiveLogins
                  ? 'User Registration is not Required to Gain Access to this App'
                  : 'User Registration is Required to Gain Access to this App'
              "
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="app.disableCustomUrls"
              :label="
                app.disableCustomUrls
                  ? 'Custom Urls for Email Confirmations and Password Resets are disabled'
                  : 'Custom Urls for Email Confirmations and Password Resets are enabled'
              "
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="isOwnersEmailConfirmed"
              :label="
                isOwnersEmailConfirmed
                  ? 'Owner\'s Email is Confirmed'
                  : 'Owner\'s Email is not Confirmed'
              "
              readonly
            ></v-checkbox>
          </v-col>
        </v-row>
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
                  @click="openEditAppDialog"
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
                  color="blue darken-1"
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
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { appService } from "@/services/appService/app.service";
import App from "@/models/app";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";
import { convertStringToDateTime } from "@/helpers/commonFunctions/commonFunctions";

export default {
  name: "AppInfoWidget",
  data: () => ({
    app: new App(),
  }),
  methods: {
    ...mapActions("appModule", [
      "updateSelectedApp",
      "updateApps",
      "removeApps",
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
              const response = await appService.resetApp(this.$data.app);

              if (response.status === 200) {
                const appsResponse = await appService.getMyApps();

                if (appsResponse.data.success) {
                  let myTempArray = [];

                  for (const app of appsResponse.data.apps) {
                    const myApp = new App(app);
                    const licenseResponse = await appService.getLicense(
                      myApp.id
                    );
                    if (licenseResponse.data.success) {
                      myApp.updateLicense(licenseResponse.data.license);
                    }
                    if (this.$data.app.id === myApp.id) {
                      this.$data.app = new App(myApp)
                    }
                    myTempArray.push(myApp);
                  }

                  this.removeApps();
                  this.updateApps(myTempArray);
                  this.updateSelectedApp(this.$data.app);

                  showToast(
                    this,
                    ToastMethods["success"],
                    response.data.message.substring(17),
                    defaultToastOptions()
                  );
                }
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
              const response = await appService.deleteApp(this.$data.app);

              if (response.status === 200) {
                const appsResponse = await appService.getMyApps();

                if (appsResponse.data.success) {
                  let myTempArray = [];

                  for (const app of appsResponse.data.apps) {
                    const myApp = new App(app);
                    const licenseResponse = await appService.getLicense(
                      myApp.id
                    );
                    if (licenseResponse.data.success) {
                      myApp.updateLicense(licenseResponse.data.license);
                    }
                    myTempArray.push(myApp);
                  }

                  this.removeApps();
                  this.updateApps(myTempArray);
                  this.updateSelectedApp(new App());

                  showToast(
                    this,
                    ToastMethods["success"],
                    response.data.message.substring(17),
                    defaultToastOptions()
                  );

                  this.$emit("close-app-widget-event", null, null);
                }
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
        "Are you sure you want to delete this app?",
        actionToastOptions(action, "delete")
      );
    },

    openEditAppDialog() {
      this.$emit("open-edit-app-dialog-event", null, null);
    },

    close() {
      this.$emit("close-app-widget-event", null, null);
    },
  },
  computed: {
    ...mapGetters("appModule", ["getSelectedApp"]),

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
          duration = "fifty-nine"
      }

      let period;

      switch (this.$data.app.timeFrame) {

        case 0:
          period = this.$data.app.accessDuration === 1 ? "second" : "seconds";
          break;

        case 1:
          period = this.$data.app.accessDuration === 1 ? "minute" : "minutes";
          break;
        
        case 2:
          period = this.$data.app.accessDuration === 1 ? "hour" : "hours";
          break;
        
        case 3:
          period = this.$data.app.accessDuration === 1 ? "day" : "days";
          break;
        
        default:
          period = this.$data.app.accessDuration === 1 ? "month" : "months";
      }

      return `Good for ${duration} ${period}`;
    },

    getDateCreated() {
      return convertStringToDateTime(this.$data.app.dateCreated);
    },

    getDateUpdated() {
      return convertStringToDateTime(this.$data.app.dateUpdated);
    },

    isOwnersEmailConfirmed() {
      const owner = this.$data.app.users.find(
        (user) => user.id === this.$data.app.ownerId
      );
      return owner.emailConfirmed;
    },
  },
  watch: {
    "$store.state.appModule.selectedApp": function () {
      this.$data.app = new App(this.getSelectedApp);
    },
  },
  created() {
    this.$data.app = new App(this.getSelectedApp);
  },
};
</script>