<template>
  <v-container fluid>
    <div class="d-flex ma-12 pa-12 justify-center" v-if="app.name === ''" >
      <v-progress-circular
        indeterminate
        color="primary"
        :size="100"
        :width="10"
        class="progress-circular"
      ></v-progress-circular>
    </div>
    <v-card elevation="6" class="mx-auto" v-if="app.name !== ''">
      <v-row class="text-center">
        <v-col cols="12">
          <v-img src="/images/banner.jpg" height="500" />
          <h1 class="text-center centered-welcome-message text-padding">Welcome to {{ app.name }}</h1>
          <v-img src="/images/logo.png" class="my-3 centered-logo" contain height="200" />
        </v-col>
      </v-row>
      <v-row>
        <h3 class="text-center text-padding">
          Sudoku Collective is a project that serves as a ready made Web API
          that you can use to learn client side technologies. The API is
          documented so you can create your own client app which can fully
          integrate with the API. My particular implementation will include
          console and Vue apps.
        </h3>
      </v-row>
    </v-card>
  </v-container>
</template>

<style scoped>
.text-padding {
  padding: 50px 70px 50px 70px;
}
/* Galaxy Fold... folded */
@media only screen and (max-width: 280px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -150%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -130%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* iPhone 5/SE */
@media only screen and (min-width: 281px) and (max-width: 320px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -140%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -110%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* Moto G4, Galaxy S5 */
@media only screen and (min-width: 321px) and (max-width: 360px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -128%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -82%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* iPhone 6/7/8, iPhone X, iPhone 12 */
@media only screen and (min-width: 361px) and (max-width: 390px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -125%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -80%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* Pixel 2, Pixel 2 XL, Galaxy Note 10+, iPhone 6/7/8 Plus */
@media only screen and (min-width: 391px) and (max-width: 414px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -120%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -70%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* Galaxy Fold - Unfolded */
@media only screen and (min-width: 415px) and (max-width: 539px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -225%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -100%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* Surface Duo - Folded */
@media only screen and (min-width: 540px) and (max-width: 716px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -110%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -55%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* Galaxy Fold - Unfolded */
@media only screen and (min-width: 717px) and (max-width: 767px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -138%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -80%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* iPad */
@media only screen and (min-width: 768px) and (max-width: 1023px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -130%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -55%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* iPad Pro */
@media only screen and (min-width: 1024px) and (max-width: 1113px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -145%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -55%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
/* Surface Duo Unfolded */
@media only screen and (min-width: 1114px) and (max-width: 1114px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -140%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -55%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
@media only screen and (min-width: 1115px) {
  /* centered welcome text */
  .centered-welcome-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -225%);
    color: white;
    text-shadow: 2px 2px #021737;
  }
  /* centered logo */
  .centered-logo {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -70%);
    filter: drop-shadow(2px 2px 2px #021737);
  }
}
</style>

<script>
/* eslint-disable no-unused-vars */
import { mapGetters } from "vuex";
import App from "@/models/app";

export default {
  name: "HomePage",
  data: () => ({
    app: new App(),
  }),
  computed: {
    ...mapGetters("settingsModule", ["getApp"]),
  },
  watch: {
    "$store.state.settingsModule.app": {
      handler: function(val, oldVal) {
        this.$data.app = new App(this.getApp);
      }
    },
  },
  created() {
    this.$data.app = new App(this.getApp);
  },
};
</script>
