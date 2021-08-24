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
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
    <AtAGlanceWidget />
    <AppsWidget
      v-if="user.isSuperUser"
      v-on:open-edit-app-form-event="openEditAppForm()"
    />
    <UsersWidget v-if="user.isSuperUser" />
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
import { mapActions } from "vuex";
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
    creatingApp: false,
    editingApp: false,
    openingAppWidgets: false,
  }),
  methods: {
    ...mapActions("settingsModule", ["updateProcessing"]),

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
  mounted() {
    this.updateProcessing(true);
    this.$data.user = new User(this.getUser);
    this.updateProcessing(false);
  },
};
</script>
