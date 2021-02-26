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
              <CreateAppButton 
                v-on:click.native="openCreateAppDialog" />
              <span class="no-apps-message" v-if="myApps.length === 0"
                >Time to Get Coding!</span
              >
              <SelectAppButton
                v-for="(myApp, index) in myApps"
                :app="myApp"
                :key="index"
                :index="index"
                v-on:click.native="appSelected(myApp.id)"
              />
            </div>
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
    <AppWidget 
      v-if="openAppWidget" 
      :app="app"
      v-on:close-app-widget-event="closeAppWidget"
      v-on:open-edit-app-dialog-event="openEditAppDialog"/>

    <v-dialog v-model="creatingApp" persistent max-width="600px">
      <CreateAppForm
        :signUpFormStatus="creatingApp"
        v-on:create-form-closed-event="closeCreateApp"
        v-on:app-created-event="appCreatedEvent"
      />
    </v-dialog>

    <v-dialog v-model="editingApp" persistent max-width="600px">
      <EditAppForm 
        v-on:close-edit-app-event="closeEditApp"/>
    </v-dialog>
  </v-container>
</template>

<style scoped>
@media only screen and (min-width: 1297px) {
  .no-apps-message {
    margin: 85px 0 0 425px;
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
    margin: 85px 0 0 50px;
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
  name: "DashboardForm",
  components: {
    CreateAppForm,
    EditAppForm,
    AppWidget,
    CreateAppButton,
    SelectAppButton
  },
  data: () => ({
    user: {},
    app: new App(),
    myApps: [],
    creatingApp: false,
    editingApp: false,
    openAppWidget: false,    
  }),
  methods: {
    ...mapActions("appModule", ["updateApps", "removeApps"]),
    openCreateAppDialog() {
      this.$data.creatingApp = true;
    },
    openEditAppDialog() {
      this.$data.editingApp = true;
    },
    closeCreateApp() {
      this.$data.creatingApp = false;
    },
    closeEditApp() {
      this.$data.editingApp = false;
    },
    appCreatedEvent(id) {
      this.$data.creatingApp = false;
      this.appSelected(id);
      showToast(
        this,
        ToastMethods["success"],
        "Successfully created app",
        defaultToastOptions()
      );
    },
    appSelected(id) {
      this.$data.openAppWidget = true;
      const app = this.getAppById(id);
      this.$data.app = app;
    },
    closeAppWidget() {
      this.$data.app = new App();
      this.$data.openAppWidget = false;
    }
  },
  computed: {
    ...mapGetters("userModule", ["getUser"]),
    ...mapGetters("appModule", ["getAppById"]),
  },
  watch: {
    "$store.state.userModule.User": function () {
      this.$data.user = new User();
      this.$data.user.clone(this.$store.getters["userModule/getUser"]);
    },
    "$store.state.appModule.Apps": function () {
      this.$data.myApps = this.$store.getters["appModule/getApps"];
    },
  },
  async created() {
    this.$data.user = new User();
    this.$data.user.clone(this.$store.getters["userModule/getUser"]);

    const response = await appService.getMyApps(new PageListModel());

    if (response.data.success) {
      this.removeApps();
      let myTempArray = [];

      for (const app of response.data.apps) {
        const myApp = new App();
        myApp.clone(app);
        const licenseResponse = await appService.getLicense(app.id);
        if (licenseResponse.data.success) {
          myApp.updateLicense(licenseResponse.data.license);
        }
        myTempArray.push(myApp);
      }

      this.updateApps(myTempArray);
    }
  },
  destroyed() {
    this.removeApps();
  },
};
</script>
