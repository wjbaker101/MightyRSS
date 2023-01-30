import { Plugin } from 'vue';

import ButtonComponent from '@wjb/vue/component/ButtonComponent.vue';
import IconComponent from '@wjb/vue/component/IconComponent.vue';
import ModalComponent from '@wjb/vue/component/ModalComponent.vue';
import FormComponent from '@wjb/vue/component/form/FormComponent.vue';
import FormSectionComponent from '@wjb/vue/component/form/FormSectionComponent.vue';
import FormInputComponent from '@wjb/vue/component/form/FormInputComponent.vue';

export const components: Plugin = {

    install(app) {
        app.component('ButtonComponent', ButtonComponent);
        app.component('IconComponent', IconComponent);
        app.component('ModalComponent', ModalComponent);
        app.component('FormComponent', FormComponent);
        app.component('FormSectionComponent', FormSectionComponent);
        app.component('FormInputComponent', FormInputComponent);
    },

};