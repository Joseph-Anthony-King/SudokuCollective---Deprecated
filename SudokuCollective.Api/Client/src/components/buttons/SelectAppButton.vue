<template>
  <v-card
    :color="evaluateColor(index)"
    class="app-button"
    @click="selectApp"
    v-if="app.name !== ''"
  >
    <v-card-title>
      <span class="select-app-title">{{ formattedAppName }}</span>
    </v-card-title>

    <v-card-text>
      <div class="select-app-title">{{ releaseStatus }}</div>
    </v-card-text>
  </v-card>
</template>

<style scoped>
.select-app-title {
  white-space: pre-wrap;
  text-decoration: none !important;
  color: white;
}
.secondary {
  color: white;
}
.error {
  color: white;
}
</style>

<script>
export default {
  name: "SelectAppButton",
  props: ["app", "index"],
  methods: {
    selectApp() {
      this.$emit("select-app-event", null, null);
    },
    evaluateColor(value) {
      return parseInt(value) % 2 === 0 ? "secondary" : "error";
    },
  },
  computed: {
    formattedAppName() {
      if (this.$props.app.name.length > 15) {
        let formattedName = "";

        const words = this.$props.app.name.split(" ");
        let index = 0;

        words.forEach((word) => {
          if (index < 4) {
            if (formattedName === "") {
              formattedName = word;
            } else {
              formattedName = formattedName + "\n" + word;
            }

            index++;
          }
        });

        return formattedName;
      } else {
        return this.$props.app.name;
      }
    },

    releaseStatus() {
      if (!this.$props.app.isActive) {
        return this.$props.app.active;
      } else {
        return this.$props.app.status;
      }
    },
  },
};
</script>
