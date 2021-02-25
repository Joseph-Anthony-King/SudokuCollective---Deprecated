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
          <v-row>
            <CreateAppButton v-on:create-app-event="openCreateAppDialog" />
            <SelectAppButton
              v-for="myApp in myApps"
              :key="myApp.id"
              :app="myApp"
            />
          </v-row>
        </v-container>
      </v-card-text>
    </v-card>

    <v-dialog v-model="creatingApp" persistent max-width="600px">
      <CreateAppForm
        :signUpFormStatus="creatingApp"
        v-on:app-created-event="closeCreateApp"
      />
    </v-dialog>
  </v-container>
</template>

<script>
import { mapActions } from "vuex";
import CreateAppForm from "@/components/forms/CreateAppForm";
import CreateAppButton from "@/components/widgets/CreateAppButton";
import SelectAppButton from "@/components/widgets/SelectAppButton";
import { appService } from "@/services/appService/app.service";
import User from "@/models/user";
import App from "@/models/app";
import PageListModel from "@/models/viewModels/pageListModel";
import { mapGetters } from "vuex";

export default {
  name: "DashboardForm",
  components: {
    CreateAppForm,
    CreateAppButton,
    SelectAppButton,
  },
  data: () => ({
    user: {},
    app: {},
    myApps: [],
    creatingApp: false,
  }),
  methods: {
    ...mapActions("appModule", ["updateApps", "removeApps"]),
    openCreateAppDialog(app) {
      this.$data.app = app;
      console.log("new app:", this.$data.app);
      this.$data.creatingApp = true;
    },
    closeCreateApp() {
      this.$data.creatingApp = false;
    },
  },
  computed: {
    ...mapGetters("userModule", ["getUser"]),
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

      response.data.apps.map(function (value, key) {
        console.log(key);
        const myApp = new App();
        myApp.clone(value);
        myTempArray.push(myApp);
      });

      this.updateApps(myTempArray);
    }
  },
  destroyed() {
    this.removeApps();
  },
};
</script>
