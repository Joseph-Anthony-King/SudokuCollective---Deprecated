<template>
  <v-container fluid>
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title v-if="user.isLoggedIn" class="justify-center"
            >Dashboard</v-card-title
          >
          <v-card-title v-if="!user.isLoggedIn" class="justify-center warning"
            >Please Log In</v-card-title
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
              v-on:click.native="openAppWidget(myApp.id)"
            />
          </div>
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
    <AppWidget
      v-if="openingAppWidget"
      v-on:close-app-widget-event="closeAppWidget"
      v-on:open-edit-app-dialog-event="openEditAppForm"
    />

    <v-dialog v-model="creatingApp" persistent max-width="600px">
      <CreateAppForm
        :signUpFormStatus="creatingApp"
        v-on:create-form-closed-event="closeCreateAppForm"
        v-on:app-created-event="appCreatedEvent"
      />
    </v-dialog>

    <v-dialog v-model="editingApp" persistent max-width="600px">
      <EditAppForm v-on:close-edit-app-event="closeEditAppForm" />
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
</style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import CreateAppForm from "@/components/forms/CreateAppForm";
import EditAppForm from "@/components/forms/EditAppForm";
import AppWidget from "@/components/widgets/AppWidget";
import CreateAppButton from "@/components/widgets/CreateAppButton";
import SelectAppButton from "@/components/widgets/SelectAppButton";
import { appService } from "@/services/appService/app.service";
import User from "@/models/user";
import App from "@/models/app";
import PageListModel from "@/models/viewModels/pageListModel";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "DashboardPage",
  components: {
    CreateAppForm,
    EditAppForm,
    AppWidget,
    CreateAppButton,
    SelectAppButton,
  },
  data: () => ({
    user: new User(),
    app: new App(),
    myApps: [],
    creatingApp: false,
    editingApp: false,
    openingAppWidget: false,
  }),
  methods: {
    ...mapActions("appModule", [
      "updateSelectedApp",
      "updateApps",
      "removeApps",
    ]),

    openCreateAppForm() {
      this.$data.creatingApp = true;
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

    openAppWidget(id) {
      this.$data.openingAppWidget = true;
      const app = this.getAppById(id);
      this.updateSelectedApp(app);
    },

    closeAppWidget() {
      this.$data.app = new App();
      this.$data.openingAppWidget = false;
    },

    appCreatedEvent(id) {
      this.$data.creatingApp = false;
      this.openAppWidget(id);
      showToast(
        this,
        ToastMethods["success"],
        "Successfully created app",
        defaultToastOptions()
      );
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getAppById", "getApps"]),
  },
  watch: {
    "$store.state.settingsModule.user": function () {
      this.$data.user = new User(this.getUser);
    },
    "$store.state.appModule.apps": function () {
      this.$data.myApps = this.getApps;
    }
  },
  async created() {
    this.$data.user = new User(this.getUser);

    const response = await appService.getMyApps();

    if (response.data.success) {
      this.removeApps();
      let myTempArray = [];

      for (const app of response.data.apps) {
        const myApp = new App(app);
        const licenseResponse = await appService.getLicense(myApp.id);
        if (licenseResponse.data.success) {
          myApp.updateLicense(licenseResponse.data.license);
        }
        myTempArray.push(myApp);
      }

      this.updateApps(myTempArray);
    }
  },
};
</script>
