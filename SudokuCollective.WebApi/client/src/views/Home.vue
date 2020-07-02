<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <HelloWorld v-bind:msg="appTitle" />
  </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from "@/components/HelloWorld.vue";

export default {
  name: "Home",
  data() {
    return {
      appTitle: `Welcome to ${process.env.VUE_APP_TITLE}`
    }
  },
  components: {
    HelloWorld
  }
};
</script>
