<template>
  <v-card elevation="6">
    <v-card-text>
      <v-container fluid>
        <span @click="close" class="material-icons close-hover"> clear </span>
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
              v-model="app.customEmailConfirmationDevUrl"
              label="Custom Development Email Confirmation Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.customEmailConfirmationLiveUrl"
              label="Custom Production Email Confirmation Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.customPasswordResetDevUrl"
              label="Custom Development Password Reset Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="app.customPasswordResetLiveUrl"
              label="Custom Production Password Reset Url"
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
              label="Game Count"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
          </v-col>
          <v-col cols="12" lg="6" xl="6">
            <v-text-field
              v-model="app.license"
              label="App License"
              prepend-icon="wysiwyg"
              append-icon="content_copy"
              @click:append="copyLicenseToClipboard"
              readonly
            ></v-text-field>
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
              label="App Is Active"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="app.inDevelopment"
              label="App Is In Development"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="app.permitSuperUserAccess"
              label="Super User has Admin Access Rights to this App"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="app.permitCollectiveLogins"
              label="User Registration is not Required to Gain Access to this App"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="app.disableCustomUrls"
              label="Disable Custom Urls for Email Confirmations and Password Resets"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="isOwnersEmailConfirmed"
              label="Owner's Email is Confirmed"
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
.app-card-title {
  text-decoration: none !important;
  color: #666666;
  cursor: pointer;
}
.close-hover:hover {
  cursor: pointer;
}
</style>

<script>
import { mapActions } from "vuex";
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
  name: "AppWidget",
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
        await navigator.clipboard.writeText(this.getLicense);
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
      this.$data.app = new App(this.$store.getters["appModule/getSelectedApp"]);
    },
  },
  created() {
    this.$data.app = new App(this.$store.getters["appModule/getSelectedApp"]);
  },
};
</script>