import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import vuetify from "./plugins/vuetify";
import Toasted from "vue-toasted";
import "./assets/css/style.css";

Vue.config.productionTip = false;

Vue.use(Toasted);

new Vue({
  router,
  store,
  vuetify,
  render: (h) => h(App),
}).$mount("#app");
