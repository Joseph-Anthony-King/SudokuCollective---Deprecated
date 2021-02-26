<template>
  <v-card>
    <v-card-title>
      <span class="headline">Edit App</span>
    </v-card-title>
    <v-form v-model="editAppFormIsValid" ref="editAppForm">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                label="User Name"
                prepend-icon="mdi-mode-edit"
                required
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
          <span>Reset the edit app form</span>
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
          <span>Close the edit app form</span>
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="blue darken-1"
              text
              @click="submit"
              :disabled="!dirty || !editProfileFormIsValid"
              v-bind="attrs"
              v-on="on"
            >
              Submit
            </v-btn>
          </template>
          <span>Submit the edit app form</span>
        </v-tooltip>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
export default {
    name: "EditAppForm",
    data: () => ({
        dirty: false
    }),
    methods: {
        resetForm() {
            this.$data.dirty = false;
        },

        close() {
            this.$emit("edit-app-event", null, null);
            this.resetForm();
        },
    }
}
</script>