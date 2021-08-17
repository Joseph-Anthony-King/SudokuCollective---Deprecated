import Vue from "vue";
import VueRouter from "vue-router";
import Home from "@/views/Home.vue";
import store from "@/store";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "Home",
    component: Home,
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "Dashboard" */ "../views/Dashboard.vue"),
    beforeEnter: checkAuth,
  },
  {
    path: "/userProfile",
    name: "UserProfile",
    component: () =>
      import(/* webpackChuckName: "UserProile" */ "../views/UserProfile.vue"),
    beforeEnter: checkAuth,
  },
  {
    path: "/sudoku",
    name: "Sudoku",
    component: () =>
      import(/* webpackChuckName: "Solve" */ "../views/Sudoku.vue"),
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

function checkAuth(to, from, next) {
  var user = store.getters["settingsModule/getUser"];
  if (user.isLoggedIn) next();
  else next("/");
}

export default router;
