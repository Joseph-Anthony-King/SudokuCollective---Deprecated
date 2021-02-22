<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <UserProfileForm />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import UserProfileForm from "@/components/pages/UserProfileForm.vue";
export default {
  name: "UserProfile",
  components: {
    UserProfileForm,
  },
};
</script>
