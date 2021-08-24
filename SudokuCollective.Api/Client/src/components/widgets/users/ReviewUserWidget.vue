<template>
  <div>
    <v-card elevation="6" class="mx-auto">
      <v-container>
        <span @click="closeReviewUserWidget" class="material-icons close-hover">
          clear
        </span>
      </v-container>
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">
            Review User: {{ user.userName }}
          </v-card-title>
          <hr class="title-spacer" />
          <UserProfileWidget :user="user" />
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
                    :color="user.isActive ? 'red darken-1' : 'blue darken-1'"
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

<style scoped></style>

<script>
/* eslint-disable no-unused-vars */
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import User from "@/models/user";
import UserProfileWidget from "@/components/widgets/users/UserProfileWidget";
import { userProvider } from "@/providers/userProvider";
import { ToastMethods } from "@/models/arrays/toastMethods";
import {
  showToast,
  defaultToastOptions,
  actionToastOptions,
} from "@/helpers/toastHelper";

export default {
  name: "ReviewUserWidget",
  components: {
    UserProfileWidget,
  },
  data: () => ({
    user: new User(),
  }),
  methods: {
    ...mapActions("userModule", ["replaceUser"]),

    closeReviewUserWidget() {
      this.$emit("close-review-user-widget-event", null, null);
    },

    async activateDeactive() {
      let action;
      let dialogText;

      if (this.$data.user.isActive) {
        action = [
          {
            text: "Yes",
            onClick: async (e, toastObject) => {
              toastObject.goAway(0);

              try {
                const response = await userProvider.deactivateUser(
                  this.$data.user.id
                );

                if (response.status === 200) {
                  this.$data.user.isActive = false;
                  this.replaceUser(this.$data.user);

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
          "Do you want to deactivate " + this.$data.user.userName + "?";
      } else {
        action = [
          {
            text: "Yes",
            onClick: async (e, toastObject) => {
              toastObject.goAway(0);

              try {
                const response = await userProvider.activateUser(
                  this.$data.user.id
                );

                if (response.status === 200) {
                  this.$data.user.isActive = true;
                  this.replaceUser(this.$data.user);

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
          "Do you want to activate " + this.$data.user.userName + "?";
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
    ...mapGetters("userModule", ["getSelectedUser"]),

    buttonText() {
      if (this.$data.user.isActive) {
        return "Deactivate";
      } else {
        return "Activate";
      }
    },

    toolTipText() {
      if (this.$data.user.isActive) {
        return "Deactivate this user";
      } else {
        return "Activate this user";
      }
    },
  },
  watch: {
    "$store.state.userModule.selectedUser": {
      handler: function (val, oldVal) {
        this.$data.user = this.getSelectedUser;
      },
    },
  },
  mounted() {
    this.$data.user = this.getSelectedUser;
  },
};
</script>
