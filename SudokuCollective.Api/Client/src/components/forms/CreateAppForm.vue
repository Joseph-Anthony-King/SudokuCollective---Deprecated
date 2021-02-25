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
import App from "@/models/app";

export default {
  name: "CreateAppForm",
  data: () => ({
    app: {},
    createAppFormIsValid: true,
    dirty: false,
  }),
  methods: {
    resetForm() {
      this.$data.app.clone(this.$store.getters["appModule/getApp"]);
      this.$data.dirty = false;
    },
    close() {
      this.$emit("app-created-event", null, null);
      this.resetForm();
    },
    submit() {
      console.log("app values:", this.$data.app);
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