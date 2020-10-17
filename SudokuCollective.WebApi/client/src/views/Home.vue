<template>
  <div class="home">
    <HomeForm />
  </div>
</template>

<script>
// @ is an alias to /src
import HomeForm from "@/components/HomeForm.vue";

export default {
  name: "Home",
  components: {
    HomeForm
  }
};
</script>
