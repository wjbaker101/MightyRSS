<template>
    <div class="collection-modal-component">
        <h2>{{ form.reference ? 'Update' : 'New' }} Collection</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Name">
                    <input type="text" placeholder="My Collection" v-model="form.name">
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent>
                <button @click="onSubmit">{{ form.reference ? 'Update' : 'Create' }}</button>
                <UserMessageComponent />
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import UserMessageComponent, { useUserMessage } from '@/components/UserMessage.component.vue';

import { apiClient } from '@/api/api-client';

import { ICollection } from '@/model/Collection.model';

const props = defineProps<{
    collection?: ICollection;
}>();

const userMessage = useUserMessage();

interface IForm {
    reference?: string;
    name: string;
}

const form = ref<IForm>({
    reference: props.collection?.reference,
    name: props.collection?.name ?? '',
});

const onSubmit = async function (): Promise<void> {
    if (form.value.reference) {
        const result = await apiClient.collections.update(form.value.reference, {
            name: form.value.name,
        });

        form.value.reference = result.reference;
        form.value.name = result.name;
    }
    else {
        const result = await apiClient.collections.add({
            name: form.value.name,
        });
        if (result instanceof Error) {
            userMessage.show(result.message);
            return;
        }

        form.value.reference = result.reference;
        form.value.name = result.name;
    }
};
</script>

<style lang="scss">
.collection-modal-component {
    width: 540px;
}
</style>