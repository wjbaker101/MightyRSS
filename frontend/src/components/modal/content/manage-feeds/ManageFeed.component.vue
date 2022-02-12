<template>
    <div class="manage-feed-component flex flex-vertical">
        <div>
            <HiddenTextBoxComponent v-model="displayTitle" @finish="onTitleFinish" />
            <br>
            <small>{{ feed.rssUrl }}</small>
        </div>
        <div class="delete-feed flex-auto" @click="onDelete">
            &times;
        </div>
    </div>
</template>

<script lang="ts">
import { computed, defineComponent, PropType } from 'vue';

import HiddenTextBoxComponent from '@/components/HiddenTextBox.component.vue';

import { FeedSource } from '@/types/FeedSource.type';
import { feedApi } from '@/api/feed/Feed.api';

export default defineComponent({
    name: 'ManageFeedComponent',

    components: {
        HiddenTextBoxComponent,
    },

    props: {
        feed: {
            type: Object as PropType<FeedSource>,
            required: true,
        },
    },

    setup(props) {
        const displayTitle = computed<string>({
            get() {
                return props.feed.titleAlias ?? props.feed.title;
            },
            set(title) {
                props.feed.titleAlias = title;
            },
        });

        return {
            displayTitle,

            async onTitleFinish(newTitle: string) {
                await feedApi.updateFeedSource(props.feed.reference, {
                    collection: props.feed.collection,
                    title: newTitle,
                });
            },
        }
    },
});
</script>

<style lang="scss">
.manage-feed-component {
    margin: 0.25rem 0;

    &:first-child {
        margin-top: 0;
    }

    &:last-child {
        margin-bottom: 0;
    }

    &:hover {
        .delete-feed {
            opacity: 1;
        }
    }

    .delete-feed {
        padding: 0 0.5rem;
        cursor: pointer;
        border-radius: 0.25rem;
        opacity: 0;
        transition: all 0.2s;

        &:hover {
            background-color: rgba(0, 0, 0, 0.2);
        }
    }
}
</style>
