import { createRouter, createWebHistory } from "vue-router";

import About from "./pages/About.vue";
import History from "./pages/History.vue";
import Home from "./pages/Home.vue";
import Month from "./pages/Month.vue";
import Logout from "./pages/Logout.vue";
import Error from "./pages/Error.vue";

export const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            name: "Home",
            path: "/",
            component: Home
        },
        {
            name: "About",
            path: "/about",
            component: About
        },
        {
            name: "History",
            path: "/history/:id",
            component: History
        },
        {
            name: "Month",
            path: "/month/:id",
            component: Month
        },
        {
            name: "Logout",
            path: "/logout",
            component: Logout
        },
        {
            name: "Error",
            path: "/**",
            component: Error
        },
    ]
})