import { Plugin } from 'vue';

import { components as wjbComponents } from '@wjb/vue/setup/components';

export const components: Plugin = {

    install(app) {
        app.use(wjbComponents);
    },

};