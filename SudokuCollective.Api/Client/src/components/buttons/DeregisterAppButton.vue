<template>
  <v-card color="warning" class="deregister-button">
    <v-card-title>
      <span class="headline">Deregister</span>
    </v-card-title>
  </v-card>
</template>

<style scoped>
.warning {
  color: red;
}

.headline {
  margin: -5px 0 0 25px;
}
</style>

<script>
export default {
  name: "DeregisterAppButton",
};
</script>
