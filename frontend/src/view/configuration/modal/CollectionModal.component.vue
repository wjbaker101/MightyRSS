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
                <button>{{ form.reference ? 'Update' : 'Create' }}</button>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { ICollection } from '@/model/Collection.model';
import { ref } from 'vue';

const props = defineProps<{
    collection?: ICollection;
}>();

interface IForm {
    reference?: string;
    name: string;
}

const form = ref<IForm>({
    reference: props.collection?.reference,
    name: props.collection?.name ?? '',
});
</script>

<style lang="scss">
.collection-modal-component {
    width: 540px;
}
</style>