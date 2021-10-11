import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router"
import Main from "../views/Main"
import File from "../views/File"
import User from "../views/User"
import Login from "../views/Login"

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "main",
    component: Main,
    redirect: "/user",
    children: [
      {
        path: "/file",
        name: "file",
        component: File
      },
      {
        path: "/user",
        name: "user",
        component: User
      }
    ]
  },
  {
    path: "/login",
    name: "login",
    component: Login
  },
  {
    path: "/about",
    name: "About",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/About.vue")
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
