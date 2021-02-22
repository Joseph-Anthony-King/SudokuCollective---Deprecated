<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
        <DashboardForm />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import DashboardForm from "@/components/pages/DashboardForm.vue";
export default {
  name: "Dashboard",
  components: {
    DashboardForm,
  },
};
</script>

<style scoped>
.container {
  width: 50%;
}
</style>
