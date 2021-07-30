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
  </div> 
</template>

<style scoped>

</style>

<script>
import _ from "lodash";
import { appProvider } from "@/providers/appProvider";
import { convertStringToDateTime } from "@/helpers/commonFunctions/commonFunctions";

export default {
  name: "AppsWidget",
  data: () => ({
    apps: [],
    search: "",
    selectedApps: [],
    headers: [
      {
        text: "Apps",
        align: "start",
        sortable: true,
        value: "name",
      },
      { text: "Id", value: "id" },
      { text: "Status", value: "status" },
      { text: "Active", value: "active" },
      { text: "Date Created", value: "dateCreated" },
    ],
    processing: false
  }),
  computed: {
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
  async created() {
    this.$data.processing = true;
    const response = await appProvider.getApps();

    if (response.success) {
      this.$data.apps = response.apps;
    }
    console.log(this.$data.apps);

    this.$data.apps.forEach((app) => {
      app["dateCreated"] = convertStringToDateTime(app.dateCreated);
    });
    this.$data.processing = false;
  }
}
</script>
