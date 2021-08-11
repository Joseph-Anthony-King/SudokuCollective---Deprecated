<template>
  <div v-if="!processing">
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">{{ title }}</v-card-title>
          <hr class="title-spacer" />
          <div class="app-buttons-scroll">
            <CreateAppButton
              :isEnabled="user.isEmailConfirmed"
              v-on:click.native="openCreateAppForm"
            />
            <span class="no-apps-message" v-if="apps.length === 0"
              >Time to Get Coding!</span
            >
            <SelectAppButton
              v-for="(app, index) in apps"
              :app="app"
              :key="index"
              :index="index"
              v-on:click.native="openAppWidgets(app.id)"
            />
          </div>
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
    <AppWidget
      v-if="openingAppWidgets"
      v-on:close-app-widget-event="closeAppWidgets"
      v-on:open-edit-app-form-event="openEditAppForm"
    />
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
</style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import CreateAppButton from "@/components/widgets/buttons/CreateAppButton";
import SelectAppButton from "@/components/widgets/buttons/SelectAppButton";
import AppWidget from "@/components/widgets/apps/AppWidget";
import { appProvider } from "@/providers/appProvider";
import App from "@/models/app";
import User from "@/models/user";

export default {
  name: "MyAppsWidget",
  components: {
    AppWidget,
    CreateAppButton,
    SelectAppButton,
  },
  data: () => ({
    user: new User(),
    app: new App(),
    openingAppWidgets: false,
    apps: [],
    processing: false,
  }),
  methods: {
    ...mapActions("appModule", ["updateUsersSelectedApp", "updateUsersApps"]),

    openCreateAppForm() {
      this.$emit("open-create-app-form-event", null, null);
    },

    openEditAppForm() {
      this.$emit("open-edit-app-form-event", null, null);
    },

    openAppWidgets(id) {
      this.$data.openingAppWidgets = true;
      this.$data.app = this.getUsersAppById(id);
      this.updateUsersSelectedApp(this.$data.app);
    },

    closeAppWidgets() {
      this.$data.openingAppWidgets = false;
      this.$data.app = new App();
      this.updateUsersSelectedApp(this.$data.app);
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", [
      "getUsersAppById",
      "getUsersApps",
      "getUsersSelectedApp",
    ]),
    title() {
      const licenses = this.$data.apps.length === 1 ? "License" : "Licenses";
      return (
        "You Currently have " + this.$data.apps.length + " App " + licenses
      );
    },
  },
  watch: {
    "$store.state.appModule.usersApps": {
      handler: function (val, oldVal) {
        this.$data.apps = this.getUsersApps;
      },
      deep: true,
    },
  },
  async created() {
    this.$data.processing = true;
    this.$data.user = this.getUser;
    const selectedApp = this.getUsersSelectedApp;

    if (selectedApp.id !== 0) {
      this.openAppWidgets(selectedApp.id);
    }

    const storeApps = this.getUsersApps;

    if (storeApps.length === 0) {
      const response = await appProvider.getMyApps();

      if (response.success) {
        response.apps.forEach((app) => {
          this.$data.apps.push(new App(app));
        });
        this.updateUsersApps(this.$data.apps);
      }
    } else {
      storeApps.forEach((store) => {
        this.$data.apps.push(store);
      });
    }
    this.$data.processing = false;
  },
};
</script>
