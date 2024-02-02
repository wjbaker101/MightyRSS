import { createRouter, createWebHistory, NavigationGuardWithThis, RouteRecordRaw } from 'vue-router';

import ConfigurationView from '@/views/configuration/Configuration.view.vue';
import LoginView from '@/views/login/Login.view.vue';
import FeedView from '@/views/feed/Feed.view.vue';

import { useAuth } from '@/use/auth.use';

const auth = useAuth();

const requireAuth: NavigationGuardWithThis<void> = function (to, from, next): void {
    if (auth.isLoggedIn() === null)
        next('/login');
    else
        next();
};

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: FeedView,
        beforeEnter: [requireAuth],
    },
    {
        path: '/login',
        component: LoginView,
    },
    {
        path: '/configuration',
        component: ConfigurationView,
        beforeEnter: [requireAuth],
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export { router };