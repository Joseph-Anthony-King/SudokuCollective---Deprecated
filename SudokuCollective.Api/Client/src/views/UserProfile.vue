<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <UserProfilePage />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import UserProfilePage from "@/components/pages/UserProfilePage.vue";
export default {
  name: "UserProfile",
  components: {
    UserProfilePage,
  },
};
</script>
