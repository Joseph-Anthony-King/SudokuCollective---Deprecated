<template>
  <v-container>
    <v-row class="text-center">
      <v-col cols="12">
        <v-img
          :src="require('../assets/logo.svg')"
          class="my-3"
          contain
          height="200"
        />
      </v-col>

      <v-col class="mb-4">
          <h1 class="display-2 font-weight-bold mb-3">
              Welcome to Sudoku Collective Admin Vue
          </h1>

          <p class="subheading font-weight-regular">
              Sudoku Collective is a project that serves as a ready made Web API that you can use 
              to learn client side technologies. The API is documented so you can create your own 
              client app which can fully integrate with the API. My particular implementation will 
              include console and Vue apps.
          </p>

          <p class="subheading font-weight-regular">
              For help and collaboration with other Vuetify developers,
              <br />please join our online
              <a href="https://community.vuetifyjs.com" target="_blank">Discord Community</a>
          </p>
      </v-col>

    </v-row>
  </v-container>
</template>
