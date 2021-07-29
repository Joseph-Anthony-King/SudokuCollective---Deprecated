<template>
  <div v-if="!processing">
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">Your Apps</v-card-title>
          <hr class="title-spacer" />
          <div class="app-buttons-scroll">
            <CreateAppButton
              :isEnabled="user.emailConfirmed"
              v-on:click.native="openCreateAppForm"
            />
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
    <v-card elevation="6" class="mx-auto" v-if="openingAppWidgets">
      <v-container>
        <span @click="closeAppWidgets" class="material-icons close-hover">
          clear
        </span>
      </v-container>
      <v-card-text>
        <v-tabs fixed-tabs>
          <v-tab href="#info"> App Info </v-tab>
          <v-tab-item value="info">
            <AppInfoWidget
              v-on:close-app-widget-event="closeAppWidgets"
              v-on:open-edit-app-dialog-event="openEditAppForm"
            />
          </v-tab-item>

          <v-tab href="#app-users"> App Users </v-tab>
          <v-tab-item value="app-users">
            <AppUsersWidget />
          </v-tab-item>

          <v-tab href="#non-app-users"> Non-App Users </v-tab>
          <v-tab-item value="non-app-users">
            <NonAppUsersWidget />
          </v-tab-item>
        </v-tabs>
      </v-card-text>
    </v-card>
  </div>
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
import AppInfoWidget from "@/components/widgets/AppInfoWidget";
import AppUsersWidget from "@/components/widgets/AppUsersWidget";
import NonAppUsersWidget from "@/components/widgets/NonAppUsersWidget";
import CreateAppButton from "@/components/widgets/CreateAppButton";
import SelectAppButton from "@/components/widgets/SelectAppButton";
import { appProvider } from "@/providers/appProvider";
import App from "@/models/app";
import User from "@/models/user";

export default {
  name: "MyAppsWidget",
  components: {
    AppInfoWidget,
    AppUsersWidget,
    NonAppUsersWidget,
    CreateAppButton,
    SelectAppButton,
  },
  data: () => ({
    user: new User(),
    app: new App(),
    openingAppWidgets: false,
    myApps: [],
    processing: false,
  }),
  methods: {
    ...mapActions("appModule", [
      "updateSelectedApp",
      "updateApps",
    ]),

    openCreateAppForm() {
      this.$emit("open-create-app-form-event", null, null);      
    },

    openEditAppForm() {
      this.$emit("open-edit-app-form-event", null, null);  
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
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", [
      "getAppById",
      "getApps",
      "getSelectedApp",
    ]),
  },
  watch: {
    "$store.state.appModule.apps": {
      handler: function (val, oldVal) {
        this.$data.myApps = this.getApps;
      },
      deep: true,
    },
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
      const response = await appProvider.getMyApps();

      if (response.success) {
        this.$data.myApps = response.apps;
        this.updateApps(this.$data.myApps);
      }
    } else {
      storeApps.forEach((store) => {
        this.$data.myApps.push(store);
      });
    }
    this.$data.processing = false;
  },
}
</script>