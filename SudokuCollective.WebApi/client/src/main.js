import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import vuetify from "./plugins/vuetify";
import VueSnackbar from "vue-snack";
import "vue-snack/dist/vue-snack.min.css";

Vue.config.productionTip = false;

Vue.use(VueSnackbar);

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount("#app");
