<template>
  <v-container fluid>
    <UserProfileForm
      v-on:edit-profile-event="openEditProfileForm" />

    <v-dialog v-model="editingProfile" persistent max-width="600px">
      <EditProfileForm
        :editProfileFormStatus="editingProfile"
        v-on:edit-user-profile-event="closeEditProfileForm"
      />
    </v-dialog>
  </v-container>
</template>

<style scoped>

</style>

<script>
/* eslint-disable no-unused-vars */
import UserProfileForm from "@/components/forms/UserProfileForm";
import EditProfileForm from "@/components/forms/EditProfileForm";

export default {
  name: "UserProfilePage",
  components: {
    UserProfileForm,
    EditProfileForm,
  },
  data: () => ({
    editingProfile: false,
  }),
  methods: {
    openEditProfileForm() {
      this.$data.editingProfile = true;
    },

    closeEditProfileForm() {
      this.$data.editingProfile = false;
    }
  }
};
</script>
