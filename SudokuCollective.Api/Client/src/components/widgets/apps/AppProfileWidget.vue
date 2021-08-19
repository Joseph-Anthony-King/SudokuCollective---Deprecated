<template>
  <v-row>
    <v-col cols="12" lg="6" xl="6">
      <v-text-field
        v-model="appId"
        label="Id"
        prepend-icon="wysiwyg"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="appName"
        label="Name"
        prepend-icon="wysiwyg"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="appLicense"
        label="App License"
        prepend-icon="wysiwyg"
        append-icon="content_copy"
        @click:append="copyLicenseToClipboard"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="getAccessPeriod"
        label="Authorization Token Access Period"
        prepend-icon="av_timer"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="appDevUrl"
        label="Development Url"
        prepend-icon="wysiwyg"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="appLiveUrl"
        label="Production Url"
        prepend-icon="wysiwyg"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="appCustomEmailConfirmationAction"
        label="Custom Email Confirmation Action"
        prepend-icon="wysiwyg"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="appCustomPasswordResetAction"
        label="Custom Password Reset Action"
        prepend-icon="wysiwyg"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="appUserCount"
        label="User Count"
        prepend-icon="wysiwyg"
        readonly
      ></v-text-field>
    </v-col>
    <v-col cols="12" lg="6" xl="6">
      <v-text-field
        v-model="appDateCreated"
        label="Date Created"
        hint="MM/DD/YYYY format"
        persistent-hint
        prepend-icon="mdi-calendar"
        readonly
      ></v-text-field>
      <v-text-field
        v-if="reviewDateUpdated(appDateUpdated)"
        v-model="appDateUpdated"
        label="Date Updated"
        hint="MM/DD/YYYY format"
        persistent-hint
        prepend-icon="mdi-calendar"
        readonly
      ></v-text-field>
      <v-checkbox
        v-model="appIsActive"
        :label="
          appIsActive
            ? 'App is Active'
            : 'App is deactivated, API requests will be denied as invalid'
        "
        readonly
      ></v-checkbox>
      <v-checkbox
        v-model="appInDevelopment"
        :label="
          appInDevelopment ? 'App is in Development' : 'App is in Production'
        "
        readonly
      ></v-checkbox>
      <v-checkbox
        v-model="appPermitSuperUserAccess"
        :label="
          appPermitSuperUserAccess
            ? 'Super User has Admin Access Rights to this App'
            : 'Super User does not have Admin Access Rights to this App'
        "
        readonly
      ></v-checkbox>
      <v-checkbox
        v-model="appPermitCollectiveLogins"
        :label="
          appPermitCollectiveLogins
            ? 'User Registration is not Required to Gain Access to this App'
            : 'User Registration is Required to Gain Access to this App'
        "
        readonly
      ></v-checkbox>
      <v-checkbox
        v-model="appDisableCustomUrls"
        :label="
          appDisableCustomUrls
            ? 'Custom Urls for Email Confirmations and Password Resets are disabled'
            : 'Custom Urls for Email Confirmations and Password Resets are enabled'
        "
        readonly
      ></v-checkbox>
      <v-checkbox
        v-model="isOwnersisEmailConfirmed"
        :label="
          isOwnersisEmailConfirmed
            ? 'Owner\'s Email is Confirmed'
            : 'Owner\'s Email is not Confirmed'
        "
        readonly
      ></v-checkbox>
    </v-col>
  </v-row>
</template>

<style scoped></style>

<script>
import { displayDateUpdated } from "@/helpers/commonFunctions/commonFunctions";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "AppProfileWidget",
  props: {
    app: {},
  },
  methods: {
    async copyLicenseToClipboard() {
      try {
        await navigator.clipboard.writeText(this.appLicense);
        showToast(
          this,
          ToastMethods["success"],
          "Copied license to clipboard",
          defaultToastOptions()
        );
      } catch (error) {
        showToast(this, ToastMethods["error"], error, defaultToastOptions());
      }
    },

    reviewDateUpdated(dateUpdated) {
      return displayDateUpdated(dateUpdated)
    }
  },
  computed: {
    appId() {
      return this.$props.app.id;
    },
    appName() {
      return this.$props.app.name;
    },
    appLicense() {
      return this.$props.app.license;
    },
    appDevUrl() {
      return this.$props.app.devUrl;
    },
    appLiveUrl() {
      return this.$props.app.liveUrl;
    },
    appCustomEmailConfirmationAction() {
      return this.$props.app.customEmailConfirmationAction;
    },
    appCustomPasswordResetAction() {
      return this.$props.app.customPasswordResetAction;
    },
    appUserCount() {
      return this.$props.app.userCount;
    },
    appDateCreated() {
      return this.$props.app.dateCreated;
    },
    appDateUpdated() {
      return this.$props.app.dateUpdated;
    },
    appIsActive() {
      return this.$props.app.isActive;
    },
    appInDevelopment() {
      return this.$props.app.inDevelopment;
    },
    appPermitSuperUserAccess() {
      return this.$props.app.permitSuperUserAccess;
    },
    appPermitCollectiveLogins() {
      return this.$props.app.permitCollectiveLogins;
    },
    appDisableCustomUrls() {
      return this.$props.app.disableCustomUrls;
    },
    getAccessPeriod() {
      let duration;

      switch (this.$props.app.accessDuration) {
        case 1:
          duration = "one";
          break;

        case 2:
          duration = "two";
          break;

        case 3:
          duration = "three";
          break;

        case 4:
          duration = "four";
          break;

        case 5:
          duration = "five";
          break;

        case 6:
          duration = "six";
          break;

        case 7:
          duration = "seven";
          break;

        case 8:
          duration = "eight";
          break;

        case 9:
          duration = "nine";
          break;

        case 10:
          duration = "ten";
          break;

        case 11:
          duration = "eleven";
          break;

        case 12:
          duration = "twelve";
          break;

        case 13:
          duration = "thirteen";
          break;

        case 14:
          duration = "fourteen";
          break;

        case 15:
          duration = "fifteen";
          break;

        case 16:
          duration = "sixteen";
          break;

        case 17:
          duration = "seventeen";
          break;

        case 18:
          duration = "eighteen";
          break;

        case 19:
          duration = "nineteen";
          break;

        case 20:
          duration = "twenty";
          break;

        case 21:
          duration = "twenty-one";
          break;

        case 22:
          duration = "twenty-two";
          break;

        case 23:
          duration = "twenty-three";
          break;

        case 24:
          duration = "twenty-four";
          break;

        case 25:
          duration = "twenty-five";
          break;

        case 26:
          duration = "twenty-six";
          break;

        case 27:
          duration = "twenty-seven";
          break;

        case 28:
          duration = "twenty-eight";
          break;

        case 29:
          duration = "twenty-nine";
          break;

        case 30:
          duration = "thirty";
          break;

        case 31:
          duration = "thirty-one";
          break;

        case 32:
          duration = "thirty-two";
          break;

        case 33:
          duration = "thirty-three";
          break;

        case 34:
          duration = "thirty-four";
          break;

        case 35:
          duration = "thirty-five";
          break;

        case 36:
          duration = "thirty-six";
          break;

        case 37:
          duration = "thirty-seven";
          break;

        case 38:
          duration = "thirty-eight";
          break;

        case 39:
          duration = "thirty-nine";
          break;

        case 40:
          duration = "fourty";
          break;

        case 41:
          duration = "fourty-one";
          break;

        case 42:
          duration = "fourty-two";
          break;

        case 43:
          duration = "fourty-three";
          break;

        case 44:
          duration = "fourty-four";
          break;

        case 45:
          duration = "fourty-five";
          break;

        case 46:
          duration = "fourty-six";
          break;

        case 47:
          duration = "fourty-seven";
          break;

        case 48:
          duration = "fourty-eight";
          break;

        case 49:
          duration = "fourty-nine";
          break;

        case 50:
          duration = "fifty";
          break;

        case 51:
          duration = "fifty-one";
          break;

        case 52:
          duration = "fifty-two";
          break;

        case 53:
          duration = "fifty-three";
          break;

        case 54:
          duration = "fifty-four";
          break;

        case 55:
          duration = "fifty-five";
          break;

        case 56:
          duration = "fifty-six";
          break;

        case 57:
          duration = "fifty-seven";
          break;

        case 58:
          duration = "fifty-eight";
          break;

        default:
          duration = "fifty-nine";
      }

      let period;

      switch (this.$props.app.timeFrame) {
        case 1:
          period = this.$props.app.accessDuration === 1 ? "second" : "seconds";
          break;

        case 2:
          period = this.$props.app.accessDuration === 1 ? "minute" : "minutes";
          break;

        case 3:
          period = this.$props.app.accessDuration === 1 ? "hour" : "hours";
          break;

        case 4:
          period = this.$props.app.accessDuration === 1 ? "day" : "days";
          break;

        default:
          period = this.$props.app.accessDuration === 1 ? "month" : "months";
      }

      return `Good for ${duration} ${period}`;
    },
    isOwnersisEmailConfirmed() {
      const owner = this.$props.app.users.find(
        (user) => user.id === this.$props.app.ownerId
      );
      return owner.isEmailConfirmed;
    },
  },
};
</script>
