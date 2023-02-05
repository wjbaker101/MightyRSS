import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import ConfigurationView from '@/views/configuration/Configuration.view.vue';
import LoginView from '@/views/login/Login.view.vue';
import FeedView from '@/views/feed/Feed.view.vue';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: FeedView,
    },
    {
        path: '/login',
        component: LoginView,
    },
    {
        path: '/configuration',
        component: ConfigurationView,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export { router }