<template>
    <div class="feed-modal-component">
        <h2>{{ form.reference ? 'Update' : 'New' }} Feed</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Url">
                    <input type="text" placeholder="https://example.com/rss" v-model="form.websiteUrl">
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent>
                <button @click="onSubmit">{{ form.reference ? 'Update' : 'Create' }}</button>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { IFeedSource } from '@/model/FeedSource.model';
import { ref } from 'vue';

const props = defineProps<{
    feedSource?: IFeedSource;
}>();

interface IForm {
    reference?: string;
    title: string;
    websiteUrl: string;
}

const form = ref<IForm>({
    reference: props.feedSource?.reference,
    title: props.feedSource?.title ?? '',
    websiteUrl: props.feedSource?.websiteUrl ?? '',
});

const onSubmit = function (): void {};
</script>

<style lang="scss">
.feed-modal-component {
    width: 540px;
}
</style>