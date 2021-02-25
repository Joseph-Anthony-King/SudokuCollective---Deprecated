<template>
  <v-container>
    <v-row>
      <v-col cols="12">
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
