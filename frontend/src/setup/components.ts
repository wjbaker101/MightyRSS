import { App } from 'vue';

import IconComponent from '@wjb/vue/component/IconComponent.vue';

export const setupComponents = function (app: App<Element>): void {
    app.component('IconComponent', IconComponent);
};