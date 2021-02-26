<template>
  <v-card elevation="6">
    <v-card-text>
      <v-container fluid>
        <span @click="close" class="material-icons close-hover"> clear </span>
        <v-card-title class="justify-center">{{ app.name }}</v-card-title>
        <hr class="title-spacer" />
        <v-row>
          <v-col cols="12" lg="6" xl="6">
            <v-text-field
              v-model="getName"
              label="Name"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="getDevUrl"
              label="Development Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="geLiveUrl"
              label="Production Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="getCustomEmailConfirmationDevUrl"
              label="Custom Development Email Confirmation Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="getCustomEmailConfirmationDevUrl"
              label="Custom Production Email Confirmation Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="getCustomPasswordResetDevUrl"
              label="Custom Development Password Reset Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="getCustomPasswordResetLiveUrl"
              label="Custom Production Password Reset Url"
              prepend-icon="wysiwyg"
              readonly
            ></v-text-field>
          </v-col>
          <v-col cols="12" lg="6" xl="6">
            <v-text-field
              v-model="getDateCreated"
              label="Date Created"
              hint="MM/DD/YYYY format"
              persistent-hint
              prepend-icon="mdi-calendar"
              readonly
            ></v-text-field>
            <v-text-field
              v-if="app.dateUpdated !== '0001-01-01T00:00:00Z'"
              v-model="getDateUpdated"
              label="Date Updated"
              hint="MM/DD/YYYY format"
              persistent-hint
              prepend-icon="mdi-calendar"
              readonly
            ></v-text-field>
            <v-checkbox
              v-model="getIsActive"
              label="App Is Active"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="getInDevelopment"
              label="App Is In Development"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="getPermitSuperUserAccess"
              label="The Super User has Admin Access Rights to this App"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="getPermitCollectiveLogins"
              label="User Registration is not Required to Gain Access to this App"
              readonly
            ></v-checkbox>
            <v-checkbox
              v-model="getDisableCustomUrls"
              label="Custom Urls for Email Confirmations and Password Resets are Disabled"
              readonly
            ></v-checkbox>
          </v-col>
        </v-row>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<style scoped>
.close-hover:hover {
  cursor: pointer;
}
</style>

<script>
import { convertStringToDateTime } from "@/helpers/commonFunctions/commonFunctions";

export default {
  name: "AppWidget",
  props: ["app"],
  methods: {
    close() {
      this.$emit("close-app-widget-event", null, null);
    },
  },
  computed: {
    getName() {
      return this.$props.app.name;
    },
    getDevUrl() {
      return this.$props.app.devUrl;
    },
    geLiveUrl() {
      return this.$props.app.liveUrl;
    },
    getCustomEmailConfirmationDevUrl() {
      return this.$props.app.customEmailConfirmationDevUrl;
    },
    getCustomEmailConfirmationLiveUrl() {
      return this.$props.app.customEmailConfirmationLiveUrl;
    },
    getCustomPasswordResetDevUrl() {
      return this.$props.app.customPasswordResetDevUrl;
    },
    getCustomPasswordResetLiveUrl() {
      return this.$props.app.customPasswordResetLiveUrl;
    },
    getDateCreated() {
      return convertStringToDateTime(this.$props.app.dateCreated);
    },
    getDateUpdated() {
      return convertStringToDateTime(this.$props.app.dateUpdated);
    },
    getIsActive() {
      return this.$props.app.isActive;
    },
    getInDevelopment() {
      return this.$props.app.inDevelopment;
    },
    getPermitSuperUserAccess() {
      return this.$props.app.permitSuperUserAccess;
    },
    getPermitCollectiveLogins() {
      return this.$props.app.permitCollectiveLogins;
    },
    getDisableCustomUrls() {
      return this.$props.app.disableCustomUrls;
    },
  },
};
</script>