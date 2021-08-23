<template>
  <div v-if="!processing">
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">
            {{ title }}
          </v-card-title>
          <hr class="title-spacer" />
          <v-card-title>
            <v-text-field
              v-model="search"
              append-icon="mdi-magnify"
              label="Search"
              single-line
              hide-details
            ></v-text-field>
          </v-card-title>
          <v-data-table
            v-model="selectedApps"
            :single-select="singleSelect"
            :headers="headers"
            :items="apps"
            show-select
            class="elevation-1"
            :search="search"
          >
          </v-data-table>
        </v-container>
      </v-card-text>
    </v-card>
    <hr />
    <v-card elevation="6">
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
                    @click="refresh"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Refresh Apps
                  </v-btn>
                </template>
                <span>Refresh all Apps</span>
              </v-tooltip>
            </v-col>
          </v-row>
        </v-container>
      </v-card-actions>
    </v-card>
    <div class="card-spacer"></div>
    <AppWidget
      v-if="selectedApps.length > 0 && selectedApps[0].ownerId === user.id"
      v-on:close-app-widget-event="closeAppWidget"
      v-on:open-edit-app-form-event="openEditAppDialog"
    />
    <ReviewAppWidget
      v-if="selectedApps.length > 0 && selectedApps[0].ownerId !== user.id"
      v-on:close-review-app-widget-event="closeAppWidget"
    />
  </div>
</template>

<style scoped></style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import { appProvider } from "@/providers/appProvider";
import AppWidget from "@/components/widgets/apps/AppWidget";
import ReviewAppWidget from "@/components/widgets/apps/ReviewAppWidget";
import App from "@/models/app";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "AppsWidget",
  components: {
    AppWidget,
    ReviewAppWidget,
  },
  data: () => ({
    user: {},
    apps: [],
    search: "",
    selectedApps: [],
    singleSelect: true,
    headers: [
      {
        text: "Apps",
        align: "start",
        sortable: true,
        value: "name",
      },
      { text: "Id", value: "id" },
      { text: "Owner", value: "owner.userName" },
      { text: "Status", value: "status" },
      { text: "Active", value: "active" },
      { text: "Custom Actions", value: "customActions" },
      { text: "User Count", value: "users.length" },
      { text: "Date Created", value: "dateCreated" },
    ],
    processing: false,
  }),
  methods: {
    ...mapActions("appModule", [
      "updateApps",
      "updateUsersSelectedApp",
      "updateSelectedApp",
    ]),

    closeAppWidget() {
      this.$data.selectedApps = [];
    },

    openEditAppDialog() {
      this.$emit("open-edit-app-form-event", null, null);
    },

    async refresh() {
      const response = await appProvider.getApps();

      if (response.success) {
        var apps = [];

        var users = this.getUsers;

        response.apps.forEach((a) => {
          const app = new App(a);
          app["owner"] = users.find(user => user.id === app.ownerId);
          apps.push(app);
        });

        this.updateApps(apps);

        showToast(
          this,
          ToastMethods["success"],
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
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getApps"]),
    ...mapGetters("userModule", ["getUsers"]),

    title() {
      const apps = this.$data.apps.length == 1 ? "App" : "Apps";

      const prodApps = this.$data.apps.filter((app) => !app.inDevelopment);

      const devApps = this.$data.apps.filter((app) => app.inDevelopment);

      const prodSummary =
        prodApps.length == 1 ? " App in Production " : " Apps in Production ";

      const devSummary =
        devApps.length == 1 ? " App in Development " : " Apps in Development ";

      return (
        this.$data.apps.length +
        " " +
        apps +
        " Created : " +
        prodApps.length +
        prodSummary +
        "and " +
        devApps.length +
        devSummary
      );
    },
  },
  watch: {
    selectedApps: {
      handler: function (val, oldVal) {
        if (val.length > 0) {
          if (oldVal.length > 0 && oldVal[0].ownerId === this.$data.user.id) {
            this.updateUsersSelectedApp(new App());
          } else {
            this.updateSelectedApp(new App());
          }
          if (val[0].ownerId === this.$data.user.id) {
            this.updateUsersSelectedApp(val[0]);
          } else {
            this.updateSelectedApp(val[0]);
          }
        } else {
          this.updateUsersSelectedApp(new App());
          this.updateSelectedApp(new App());
        }
      },
    },
  },
  async created() {
    this.$data.processing = true;
    this.$data.user = this.getUser;
    this.$data.apps = this.getApps;
    this.$data.processing = false;
  },
};
</script>
