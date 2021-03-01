<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <DashboardPage />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import DashboardPage from "@/components/pages/DashboardPage.vue";
export default {
  name: "Dashboard",
  components: {
    DashboardPage,
  },
};
</script>
