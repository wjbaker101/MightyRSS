<template>
    <div class="feed-modal-component">
        <h2>{{ form.reference ? 'Update' : 'New' }} Feed</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Url">
                    <input type="text" placeholder="https://example.com/rss" v-model="form.websiteUrl">
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent class="flex gap">
                <button class="flex-auto" @click="onSubmit">{{ form.reference ? 'Update' : 'Create' }}</button>
                <DeleteButtonComponent v-if="form.reference" class="flex-auto" @delete="onDelete" />
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { useRss } from '@/use/rss.use';
import { useModal } from '@wjb/vue/use/modal.use';

import { IFeedSource } from '@/model/FeedSource.model';

const props = defineProps<{
    feedSource?: IFeedSource;
}>();

const rss = useRss();
const modal = useModal();

interface IForm {
    reference?: string;
    websiteUrl: string;
}

const form = ref<IForm>({
    reference: props.feedSource?.reference,
    websiteUrl: props.feedSource?.websiteUrl ?? '',
});

const onSubmit = async function (): Promise<void> {
    if (form.value.websiteUrl.length < 5)
        return;

    await rss.addSource(form.value.websiteUrl);

    modal.hide();
};

const onDelete = async function (): Promise<void> {
    if (!form.value.reference)
        return;

    await rss.deleteSource(form.value.reference);

    modal.hide();
};
</script>

<style lang="scss">
.feed-modal-component {
    width: 540px;
}
</style>