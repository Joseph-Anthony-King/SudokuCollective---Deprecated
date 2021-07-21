<template>
  <v-container fluid>
    <v-overlay
      :value="processing"
    >
      <v-progress-circular
        indeterminate
        color="primary"
        :size="100"
        :width="10"
        class="progress-circular"
      ></v-progress-circular>
    </v-overlay>
    <v-card elevation="6" class="mx-auto" v-if="!processing">
      <v-card-text>
        <v-container fluid>
          <v-card-title v-if="user.isLoggedIn" class="justify-center"
            >Dashboard</v-card-title
          >
          <v-card-title v-if="!user.isLoggedIn" class="justify-center warning"
            >Please Log In</v-card-title
          >
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
    <v-card elevation="6" class="mx-auto" v-if="!processing">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center"
            >Your Apps</v-card-title
          >
          <hr class="title-spacer" />
          <div class="app-buttons-scroll">
            <CreateAppButton v-on:click.native="openCreateAppForm" />
            <span class="no-apps-message" v-if="myApps.length === 0"
              >Time to Get Coding!</span
            >
            <SelectAppButton
              v-for="(myApp, index) in myApps"
              :app="myApp"
              :key="index"
              :index="index"
              v-on:click.native="openAppWidgets(myApp.id)"
            />
          </div>
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
    <v-card elevation="6" v-if="openingAppWidgets">
      <v-container>        
        <span @click="closeAppWidgets" class="material-icons close-hover"> clear </span>
      </v-container>
      <template>
        <v-tabs
          fixed-tabs>
          <v-tab href="#info">
            App Info
          </v-tab>
          <v-tab-item value="info">
            <AppInfoWidget
              v-on:close-app-widget-event="closeAppWidgets"
              v-on:open-edit-app-dialog-event="openEditAppForm"
            />
          </v-tab-item>

          <v-tab href="#app-users">
            App Users
          </v-tab>
          <v-tab-item value="app-users">
            <AppUsersWidget />
          </v-tab-item>

          <v-tab href="#non-app-users">
            Non-App Users
          </v-tab>
          <v-tab-item value="non-app-users">
            <NonAppUsersWidget />
          </v-tab-item>
        </v-tabs>
      </template>
    </v-card>
    <div class="card-spacer" v-if="openingAppWidgets"></div>
    <v-dialog v-model="creatingApp" persistent max-width="600px">
      <CreateAppForm
        :signUpFormStatus="creatingApp"
        v-on:create-form-closed-event="closeCreateAppForm"
        v-on:app-created-event="appCreatedEvent"
      />
    </v-dialog>
    <v-card elevation="6" class="mx-auto" v-if="!processing">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center"
            >Apps For Which You've Registered</v-card-title
          >
          <hr class="title-spacer" />
          <div class="app-buttons-scroll">
            <SelectAppButton
              v-for="(app, index) in registeredApps"
              :app="app"
              :key="index"
              :index="index"
              v-on:click.native="appAvailable(app) ? openUrl(app) : null"
            />
          </div>
        </v-container>
      </v-card-text>
    </v-card>

    <v-dialog v-model="editingApp" persistent max-width="1200px">
      <EditAppForm 
        :editAppFormStatus="editingApp"
        v-on:close-edit-app-event="closeEditAppForm" />
    </v-dialog>
  </v-container>
</template>

<style scoped>
@media only screen and (min-width: 1297px) {
  .no-apps-message {
    margin: 85px 0 0 420px;
    font-size: xx-large;
  }
}
@media only screen and (max-width: 1296px) {
  .no-apps-message {
    margin: 85px 0 0 200px;
    font-size: xx-large;
  }
}
@media only screen and (max-width: 1259px) {
  .no-apps-message {
    margin: 85px 0 0 60px;
    font-size: xx-large;
  }
}
@media only screen and (max-width: 699px) {
  .no-apps-message {
    margin: 85px 0 0 5px;
    font-size: medium;
  }
}
.app-buttons-scroll {
  display: flex;
  overflow-x: auto;
}
.close-hover:hover {
  cursor: pointer;
}
</style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import CreateAppForm from "@/components/forms/CreateAppForm";
import EditAppForm from "@/components/forms/EditAppForm";
import AppInfoWidget from "@/components/widgets/AppInfoWidget";
import AppUsersWidget from "@/components/widgets/AppUsersWidget";
import NonAppUsersWidget from "@/components/widgets/NonAppUsersWidget";
import CreateAppButton from "@/components/widgets/CreateAppButton";
import SelectAppButton from "@/components/widgets/SelectAppButton";
import { appService } from "@/services/appService/app.service";
import User from "@/models/user";
import App from "@/models/app";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "DashboardPage",
  components: {
    CreateAppForm,
    EditAppForm,
    AppInfoWidget,
    AppUsersWidget,
    NonAppUsersWidget,
    CreateAppButton,
    SelectAppButton,
  },
  data: () => ({
    user: new User(),
    app: new App(),
    myApps: [],
    registeredApps: [],
    processing: false,
    creatingApp: false,
    editingApp: false,
    openingAppWidgets: false,
  }),
  methods: {
    ...mapActions("appModule", [
      "updateSelectedApp",
      "updateApps",
      "updateRegisteredApps",
    ]),

    openCreateAppForm() {
      if (!this.$data.user.isAdmin) {
        showToast(
          this,
          ToastMethods["error"],
          "You don't have admin privileges, please review your user profile",
          defaultToastOptions()
        );
      } else if (!this.$data.user.emailConfirmed) {
        showToast(
          this,
          ToastMethods["error"],
          `Please review ${this.$data.user.email} and confirm your email`,
          defaultToastOptions()
        );
      } else {
        this.$data.creatingApp = true;
      }
    },

    closeCreateAppForm() {
      this.$data.creatingApp = false;
    },

    openEditAppForm() {
      this.$data.editingApp = true;
    },
    
    closeEditAppForm() {
      this.$data.editingApp = false;
    },

    openAppWidgets(id) {
      this.$data.openingAppWidgets = true;
      this.$data.app = this.getAppById(id);
      this.updateSelectedApp(this.$data.app);
    },

    closeAppWidgets() {
      this.$data.openingAppWidgets = false;
      this.$data.app = new App();
      this.updateSelectedApp(this.$data.app);
    },

    appCreatedEvent(id) {
      this.$data.creatingApp = false;
      this.openAppWidgets(id);
      showToast(
        this,
        ToastMethods["success"],
        "Successfully created app",
        defaultToastOptions()
      );
    },
    appAvailable(app) {
      if (app.isActive) {
        if (!app.inDevelopment) {
          if (app.liveUrl !== "") {
            return true;
          } else {
            return false;
          }
        }
      } else {
        return false;
      }
    },
    openUrl(app) {
      window.open(app.liveUrl, "_blank");
    }
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", [
      "getAppById", 
      "getApps",
      "getRegisteredApps",
      "getSelectedApp"]),
  },
  watch: {
    "$store.state.settingsModule.user": {
      handler: function(val, oldVal) {
        this.$data.user = new User(this.getUser);
      }
    },
    "$store.state.appModule.apps": {
      handler: function (val, oldVal) {
        this.$data.myApps = this.getApps;
      },
      deep: true
    }
  },
  async created() {
    this.$data.processing = true;
    this.$data.user = new User(this.getUser);

    const selectedApp = this.getSelectedApp;

    if (selectedApp.id !== 0) {
      this.openAppWidgets(selectedApp.id);
    }

    const storeApps = this.getApps;
    
    if (storeApps.length === 0) {

      const response = await appService.getMyApps();

      if (response.data.success) {
        let tempArray = [];

        for (const app of response.data.apps) {
          const myApp = new App(app);
          const licenseResponse = await appService.getLicense(myApp.id);
          if (licenseResponse.data.success) {
            myApp.updateLicense(licenseResponse.data.license);
          }
          tempArray.push(myApp);
        }
        
        // Load the users per app
        for (const app of tempArray) {
          const appUsersResponse = await appService.getAppUsers(app.id);
          console.log("appUsersResponse: ", appUsersResponse);
          appUsersResponse.data.users.forEach((user) => {
            const tempUser = new User(user);
            app.users.push(tempUser);
          });
        }

        this.updateApps(tempArray);
      }

    } else {
      
      storeApps.forEach((store) => {

        this.$data.myApps.push(store);
      });
    }

    const storeRegisteredApps = this.getRegisteredApps;

    if (storeRegisteredApps.length === 0) {
      const response = await appService.getRegisteredApps(this.$data.user.id);

      if (response.data.success) {       
        let tempArray = [];

        for (const app of response.data.apps) {
          const registeredApp = new App(app);
          tempArray.push(registeredApp)
        }

        this.updateRegisteredApps(tempArray);
      }
    }
      
    storeRegisteredApps.forEach((store) => {

      this.$data.registeredApps.push(store);
    });
    
    this.$data.processing = false;
  },
};
</script>
