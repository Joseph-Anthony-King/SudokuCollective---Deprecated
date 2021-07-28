<template>
  <v-card :class="isEnabled ? 'primary' : 'disabled'" class="app-button">
    <v-card-title>
      <span class="headline">Create App</span>
    </v-card-title>
  </v-card>
</template>

<style scoped>
.primary {
  color: white;
}

.disabled {
  background-color: #75acd0;
  color: gray;
}

.headline {
  margin: 65px 0 0 20px;
}
</style>

<script>
export default {
  name: "CreateAppButton",
  props: {
    isEnabled: Boolean,
  },
};
</script>
