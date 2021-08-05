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
    <div class="card-spacer"></div>
    <ReviewAppWidget 
      v-if="selectedApps.length > 0"/>
  </div> 
</template>

<style scoped>

</style>

<script>
/* eslint-disable no-unused-vars */
import _ from "lodash";
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import ReviewAppWidget from "@/components/widgets/ReviewAppWidget";
import App from "@/models/app";

export default {
  name: "AppsWidget",
  components: {
    ReviewAppWidget,
  },
  data: () => ({
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
      { text: "Owner", value : "owner.userName" },
      { text: "Status", value: "status" },
      { text: "Active", value: "active" },
      { text: "User Count", value: "users.length" },
      { text: "Date Created", value: "dateCreated" },
    ],
    processing: false
  }),
  methods: {
    ...mapActions("appModule", ["updateSelectedApp"]),
  },
  computed: {
    ...mapGetters("appModule", ["getApps"]),

    title() {
      const apps = this.$data.apps.length == 1 ? "App" : "Apps";
      
      const prodApps = _.filter(
        this.$data.apps, function (app) {
          return !app.inDevelopment;
        }
      );
      
      const devApps = _.filter(
        this.$data.apps, function (app) {
          return app.inDevelopment;
        }
      );

      const prodSummary = prodApps.length == 1 ? " App in Production " : " Apps in Production ";

      const devSummary = devApps.length == 1 ? " App in Development " : " Apps in Development ";

      return this.$data.apps.length + " " + apps + " Created : " 
        + prodApps.length + prodSummary  + "and " 
        + devApps.length + devSummary;
    }
  },
  watch: {
    "selectedApps": {
      handler: function (val, oldVal) {
        if (val.length > 0){
          this.updateSelectedApp(val[0]);
        } else {
          this.updateSelectedApp(new App());
        }
      },
    },
  },
  async created() {
    this.$data.processing = true;
    this.$data.apps = this.getApps;
    this.$data.processing = false;
  }
}
</script>
