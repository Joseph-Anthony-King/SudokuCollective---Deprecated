<template>
  <v-card color="secondary" class="app-button" @click="selectApp">
    <v-card-title>
      <span class="select-app-title" v-if="!app.isActive">{{
        formattedAppName
      }}</span>
      <a
        :href="app.devUrl"
        target="blank"
        class="select-app-title"
        v-if="app.inDevelopment && app.isActive"
      >
        <span>{{ formattedAppName }}</span>
      </a>
      <a
        :href="app.liveUrl"
        target="blank"
        class="select-app-title"
        v-if="!app.inDevelopment && app.isActive"
      >
        <span>{{ formattedAppName }}</span>
      </a>
      <span>Users: {{ app.userCount }}</span>
      <span v-if="app.inDevelopment">In Development</span>
      <span v-if="!app.inDevelopment">In Production</span>
      <span v-if="!app.isActive">Inactive</span>
    </v-card-title>
  </v-card>
</template>

<style scoped>
.select-app-title {
  text-decoration: none !important;
  color: white;
}
.secondary {
  color: white;
}
</style>

<script>
export default {
  name: "SelectAppButton",
  props: ["app"],
  methods: {
    selectApp() {
      this.$emit("select-app-event", null, null);
    },
  },
  computed: {
    formattedAppName() {
      let formattedName = "";

      const words = this.$props.app.name.split(" ");

      words.forEach((word) => (formattedName = formattedName + "\n" + word));

      console.log(formattedName);

      return formattedName;
    },
  },
};
</script>