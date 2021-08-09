<template>
  <div>
    <v-card elevation="6" class="mx-auto">
      <v-container>
        <span @click="closeReviewAppWidget" class="material-icons close-hover">
          clear
        </span>
      </v-container>
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">
            Review App: {{ app.name }}
          </v-card-title>
          <hr class="title-spacer" />
          <AppProfileWidget :app="app" />
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
                    :color="app.isActive ? 'red darken-1' : 'blue darken-1'"
                    text
                    @click="activateDeactive"
                    v-bind="attrs"
                    v-on="on"
                  >
                    {{ buttonText }}
                  </v-btn>
                </template>
                <span>{{ toolTipText }}</span>
              </v-tooltip>
            </v-col>
          </v-row>
        </v-container>
      </v-card-actions>
    </v-card>
    <div class="card-spacer"></div>
  </div>
</template>

<style></style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import App from "@/models/app";
import AppProfileWidget from "@/components/widgets/AppProfileWidget";
import { appProvider } from "@/providers/appProvider";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

export default {
  name: "ReviewAppWidget",
  components: {
    AppProfileWidget,
  },
  data: () => ({
    app: new App(),
  }),
  methods: {
    ...mapActions("appModule", ["replaceApp"]),

    closeReviewAppWidget() {
      this.$emit("close-review-app-widget-event", null, null);
    },

    async activateDeactive() {
      let action;
      let dialogText;

      if (this.$data.app.isActive) {
        action = [
          {
            text: "Yes",
            onClick: async (e, toastObject) => {
              toastObject.goAway(0);

              try {
                const response = await appProvider.deactivateApp(
                  this.$data.app.id
                );

                if (response.status === 200) {
                  this.$data.app.isActive = false;
                  this.$data.app.active = "Deactivated";
                  var tempApp = this.$data.app;
                  this.getUsers.forEach((user) => {
                    if (user.id === this.$data.app.ownerId) {
                      tempApp["owner"] = user;
                    }
                  });
                  this.replaceApp(tempApp);

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
        dialogText =
          "Do you want to deactivate " + this.$data.app.name + "?";
      } else {
        action = [
          {
            text: "Yes",
            onClick: async (e, toastObject) => {
              toastObject.goAway(0);

              try {
                const response = await appProvider.activateApp(
                  this.$data.app.id
                );

                if (response.status === 200) {
                  this.$data.app.isActive = true;
                  this.$data.app.active = "Active";
                  var tempApp = this.$data.app;
                  this.getUsers.forEach((user) => {
                    if (user.id === this.$data.app.ownerId) {
                      tempApp["owner"] = user;
                    }
                  });
                  this.replaceApp(tempApp);

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
        dialogText =
          "Do you want to activate " + this.$data.app.name + "?";
      }

      showToast(
        this,
        ToastMethods["show"],
        dialogText,
        actionToastOptions(action, "mode_edit")
      );
    },
  },
  computed: {
    ...mapGetters("appModule", ["getSelectedApp"]),
    ...mapGetters("userModule", ["getUsers"]),

    buttonText() {
      if (this.$data.app.isActive) {
        return "Deactivate";
      } else {
        return "Activate";
      }
    },

    toolTipText() {
      if (this.$data.app.isActive) {
        return "Deactivate this app";
      } else {
        return "Activate this app";
      }
    },
  },
  watch: {
    "$store.state.appModule.selectedApp": {
      handler: function (val, oldVal) {
        this.$data.app = this.getSelectedApp;
      },
    },
  },
  created() {
    this.$data.app = this.getSelectedApp;
  },
};
</script>
