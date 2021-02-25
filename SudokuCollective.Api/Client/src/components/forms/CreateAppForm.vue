<template>
  <v-card>
    <v-card-title>
      <span class="headline">Create App</span>
    </v-card-title>
    <v-form v-model="createAppFormIsValid" ref="createAppForm">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="app.name"
                label="Name"
                prepend-icon="mdi-account-edit"
                :rules="stringRequiredRules"
                required
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="app.devUrl"
                label="Development Url (Not Required)"
                prepend-icon="mdi-account-edit"
                :rules="urlRules"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="app.liveUrl"
                label="Live Url (Not Required)"
                prepend-icon="mdi-account-edit"
                :rules="urlRules"
                @click="!dirty ? (dirty = true) : null"
                @focus="!dirty ? (dirty = true) : null"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="resetForm"
              v-bind="attrs"
              v-on="on"
            >
              Reset
            </v-btn>
          </template>
          <span>Reset the create app form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="close"
              v-bind="attrs"
              v-on="on"
            >
              close
            </v-btn>
          </template>
          <span>Close the create app form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="submit"
              :disabled="!dirty || !createAppFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Submit
            </v-btn>
          </template>
          <span>Submit the create app form</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
/* eslint-disable no-useless-escape */
import { mapActions } from "vuex";
import { appService } from "@/services/appService/app.service";
import CreateAppModel from "@/models/viewModels/createAppModel";
import App from "@/models/app";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export default {
  name: "CreateAppForm",
  data: () => ({
    app: {},
    createAppFormIsValid: true,
    dirty: false,
  }),
  methods: {
    ...mapActions("appModule", ["addApp"]),
    resetForm() {
      this.$data.app.clone(this.$store.getters["appModule/getApp"]);
      this.$data.dirty = false;
    },
    close() {
      this.$emit("app-creat-form-closed-event", null, null);
      this.resetForm();
    },
    async submit() {
      if (this.getCreateAppFormIsValid) {
        try {
          const response = await appService.postLicense(new CreateAppModel(
            this.$data.app.name,
            this.$data.app.devUrl,
            this.$data.app.liveUrl
          ));

          if (response.status === 201) {
            this.$data.app.clone(response.data.app);

            this.addApp(this.$data.app);

            this.resetCreateAppFormIsValid;
            
            this.resetForm();
      
            this.$emit("app-created-event", this.$data.app.name, null);

          } else if (response.status === 404) {
              showToast(
                this,
                ToastMethods["error"],
                response.data.message.substring(17),
                defaultToastOptions()
              );
          } else {
            showToast(
              this,
              ToastMethods["error"],
              response.data.message,
              defaultToastOptions()
            );
          }
          
        } catch (error) {
          showToast(this, ToastMethods["error"], error, defaultToastOptions());
        }
      }
    },
  },
  computed: {
    stringRequiredRules() {
      return [(v) => !!v || "Value is required"];
    },

    urlRules() {
      const regex = /https?:[0-9]*\/\/[\w!?/\+\-_~=;\.,*&@#$%\(\)\'\[\]]+/;
      return [
        (v) =>
          !v ||
          regex.test(v) ||
          "Must be a valid url",
      ];
    },

    resetCreateAppFormIsValid() {
      return !this.createAppFormIsValid;
    },

    getCreateAppFormIsValid() {
      return this.createAppFormIsValid;
    },
  },
  created() {
    this.$data.app = new App();
    this.$data.app.clone(this.$store.getters["appModule/getApp"]);
  },
};
</script>