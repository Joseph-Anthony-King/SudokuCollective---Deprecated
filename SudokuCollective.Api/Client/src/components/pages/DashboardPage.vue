<template>
  <v-container fluid>
    <v-overlay :value="processing">
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
    <AtAGlanceWidget />
    <UsersWidget v-if="user.isSuperUser" />
    <AppsWidget
      v-if="user.isSuperUser"
      v-on:open-edit-app-form-event="openEditAppForm()"
    />
    <MyAppsWidget
      v-if="!user.isSuperUser"
      v-on:open-create-app-form-event="openCreateAppForm()"
      v-on:open-edit-app-form-event="openEditAppForm()"
    />
    <MyRegisteredAppsWidget v-if="!user.isSuperUser" />
    <v-dialog v-model="creatingApp" persistent max-width="600px">
      <CreateAppForm
        :signUpFormStatus="creatingApp"
        v-on:create-form-closed-event="closeCreateAppForm"
        v-on:app-created-event="appCreatedEvent"
      />
    </v-dialog>
    <v-dialog v-model="editingApp" persistent max-width="1200px">
      <EditAppForm
        :editAppFormStatus="editingApp"
        v-on:edit-app-closed-event="closeEditAppForm"
      />
    </v-dialog>
  </v-container>
</template>

<style scoped></style>

<script>
/* eslint-disable no-unused-vars */
import { mapGetters } from "vuex";
import CreateAppForm from "@/components/forms/CreateAppForm";
import EditAppForm from "@/components/forms/EditAppForm";
import AtAGlanceWidget from "@/components/widgets/AtAGlanceWidget";
import AppsWidget from "@/components/widgets/apps/AppsWidget";
import UsersWidget from "@/components/widgets/users/UsersWidget";
import MyAppsWidget from "@/components/widgets/apps/MyAppsWidget";
import MyRegisteredAppsWidget from "@/components/widgets/apps/MyRegisteredAppsWidget";
import App from "@/models/app";
import User from "@/models/user";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "DashboardPage",
  components: {
    CreateAppForm,
    EditAppForm,
    AtAGlanceWidget,
    AppsWidget,
    UsersWidget,
    MyAppsWidget,
    MyRegisteredAppsWidget,
  },
  data: () => ({
    user: new User(),
    app: new App(),
    processing: false,
    creatingApp: false,
    editingApp: false,
    openingAppWidgets: false,
  }),
  methods: {
    openCreateAppForm() {
      if (!this.$data.user.isAdmin) {
        showToast(
          this,
          ToastMethods["error"],
          "You don't have admin privileges, please review your user profile",
          defaultToastOptions()
        );
      } else if (!this.$data.user.isEmailConfirmed) {
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

    appCreatedEvent() {
      this.$data.creatingApp = false;
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
  },
  watch: {
    "$store.state.settingsModule.user": {
      handler: function (val, oldVal) {
        this.$data.user = new User(this.getUser);
      },
    },
  },
  created() {
    this.$data.processing = true;
    this.$data.user = new User(this.getUser);

    this.$data.processing = false;
  },
};
</script>
