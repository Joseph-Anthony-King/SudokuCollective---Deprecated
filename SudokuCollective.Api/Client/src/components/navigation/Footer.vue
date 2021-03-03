<template>
  <v-container>
    <v-row class="text-right">
      <v-col cols="12">
        <div>
          © Copyright {{ getCopyrightYear }}:
          <a
            href="https://www.linkedin.com/in/joseph-king-23650256/"
            target="_blank"
            title="LinkedIn Profile: Joseph Anthony King"
          >
            Joseph-Anthony-King</a
          >
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "Footer",
  computed: {
    getCopyrightYear() {
      const now = new Date();
      return now.getFullYear();
    }
  }
};
</script>
