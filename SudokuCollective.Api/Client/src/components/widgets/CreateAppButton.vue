<template>
  <v-card color="primary" class="app-button" @click="createApp">
    <v-card-title>
      <span class="headline">Create App</span>
    </v-card-title>
  </v-card>
</template>

<style scoped>
.primary {
  color: white;
}

.headline {
  margin: 65px 0 0 20px;
}
</style>

<script>
import { mapActions } from "vuex";
import App from "@/models/app";

export default {
  name: "CreateAppButton",
  methods: {
    ...mapActions("appModule", ["updateApp"]),
    createApp() {
      let app = new App();
      this.updateApp(app);
      this.$emit("create-app-event", null, null);
    }
  }
}
</script>