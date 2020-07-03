<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
        <LoginForm />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import LoginForm from "@/components/LoginForm.vue";
  export default {
    name: "Login",
    components: {
      LoginForm
    }
  }
</script>

<style scoped>
.container {
  width: 50%;
}
</style>