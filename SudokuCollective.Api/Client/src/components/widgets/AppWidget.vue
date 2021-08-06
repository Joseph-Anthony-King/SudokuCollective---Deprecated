<template>
  <div>
    <v-card elevation="6" class="mx-auto">
      <v-container>
        <span @click="closeAppWidgets" class="material-icons close-hover">
          clear
        </span>
      </v-container>
      <v-card-text>
        <v-tabs fixed-tabs>
          <v-tab href="#info"> App Info </v-tab>
          <v-tab-item value="info">
            <AppInfoWidget
              v-on:close-app-widget-event="closeAppWidgets"
              v-on:open-edit-app-form-event="openEditAppForm" />
          </v-tab-item>

          <v-tab href="#app-users"> App Users </v-tab>
          <v-tab-item value="app-users">
            <AppUsersWidget />
          </v-tab-item>

          <v-tab href="#non-app-users"> Non-App Users </v-tab>
          <v-tab-item value="non-app-users">
            <NonAppUsersWidget />
          </v-tab-item>
        </v-tabs>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
  </div>  
</template>

<style>
</style>

<script>
import AppInfoWidget from "@/components/widgets/AppInfoWidget";
import AppUsersWidget from "@/components/widgets/AppUsersWidget";
import NonAppUsersWidget from "@/components/widgets/NonAppUsersWidget";

export default {
  name: "AppWidget",
  components: {
    AppInfoWidget,
    AppUsersWidget,
    NonAppUsersWidget,
  },
  methods: {

    openCreateAppForm() {
      this.$emit("open-create-app-form-event", null, null);      
    },

    openEditAppForm() {
      this.$emit("open-edit-app-form-event", null, null);  
    },

    closeAppWidgets() {
      this.$emit("close-app-widget-event", null, null); 
    },
  }
}
</script>
