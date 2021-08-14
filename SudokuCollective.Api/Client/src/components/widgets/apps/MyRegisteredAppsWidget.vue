<template>
  <div v-if="!processing">
    <v-card elevation="6" class="mx-auto" v-if="!processing">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">{{ title }}</v-card-title>
          <hr class="title-spacer" />
          <div class="app-buttons-scroll">
            <div v-for="(app, index) in apps" v-bind:key="app.id">
              <SelectAppButton
                :app="app"
                :key="index"
                :index="index"
                v-on:click.native="appAvailable(app) ? openUrl(app) : null"
              />
              <DeregisterAppButton
                v-if="app.id !== 1"
                v-on:click.native="deregister(app)"
              />
            </div>
          </div>
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
                    Refresh
                  </v-btn>
                </template>
                <span>Refresh your registered apps</span>
              </v-tooltip>
            </v-col>
          </v-row>
        </v-container>
      </v-card-actions>
    </v-card>
  </div>
</template>

<style scoped>
.app-buttons-scroll {
  display: flex;
  overflow-x: auto;
}
</style>

<script>
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import SelectAppButton from "@/components/widgets/buttons/SelectAppButton";
import DeregisterAppButton from "@/components/widgets/buttons/DeregisterAppButton";
import { appProvider } from "@/providers/appProvider";
import App from "@/models/app";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

export default {
  name: "MyRegisteredAppsWidget",
  components: {
    SelectAppButton,
    DeregisterAppButton,
  },
  data: () => ({
    user: new User(),
    apps: [],
    processing: false,
  }),
  methods: {
    ...mapActions("appModule", [
      "updateRegisteredApps",
      "replaceRegisteredApps",
    ]),

    appAvailable(app) {
      if (app.isActive) {
        if (!app.inDevelopment) {
          if (app.liveUrl !== "") {
            return true;
          } else {
            return false;
          }
        } else {
          showToast(
            this,
            ToastMethods["error"],
            "App is still in development",
            defaultToastOptions()
          );
        }
      } else {
        return false;
      }
    },

    openUrl(app) {
      window.open(app.liveUrl, "_blank");
    },

    async deregister(app) {
      const action = [
        {
          text: "Yes",
          onClick: async (e, toastObject) => {
            toastObject.goAway(0);

            try {
              const response = await appProvider.removeUser(
                app.id,
                this.$data.user.id
              );

              if (response.success) {
                const appsResponse = await appProvider.getRegisteredApps(
                  this.$data.user.id
                );

                if (appsResponse.success) {
                  let apps = [];

                  appsResponse.apps.forEach((app) => {
                    apps.push(new App(app));
                  });

                  this.replaceRegisteredApps(apps);
                  this.$data.apps = apps;
                }
                showToast(
                  this,
                  ToastMethods["success"],
                  "You have been deregistered from " + app.name,
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
            this.$data.submitInvoked = false;
          },
        },
      ];

      const dialogText = "Do you want to deregister from " + app.name + "?";

      showToast(
        this,
        ToastMethods["show"],
        dialogText,
        actionToastOptions(action, "mode_edit")
      );
    },

    async refresh() {
      const response = await appProvider.getRegisteredApps(this.$data.user.id);

      try {
        if (response.success) {
          this.replaceRegisteredApps(response.apps);
          this.$data.apps = response.apps;

          showToast(
            this,
            ToastMethods["success"],
            "Your registered apps have been refreshed",
            defaultToastOptions()
          );
        } else {
          showToast(
            this,
            ToastMethods["error"],
            "Your registered apps have not been refreshed",
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
    }
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", ["getRegisteredApps"]),
    title() {
      const apps = this.$data.apps.length === 1 ? "App" : "Apps";
      return (
        "You are Currently Registered with " +
        this.$data.apps.length +
        " " +
        apps
      );
    },
  },
  async created() {
    this.$data.processing = true;
    this.$data.user = new User(this.getUser);

    const storeRegisteredApps = this.getRegisteredApps;

    if (storeRegisteredApps.length === 0) {
      const response = await appProvider.getRegisteredApps(this.$data.user.id);

      if (response.success) {
        this.updateRegisteredApps(response.apps);
        this.$data.apps = response.apps;
      }
    } else {
      this.$data.apps = storeRegisteredApps;
    }

    this.$data.processing = false;
  },
};
</script>
