<template>
    <div class="feed-source-component">
        <a :href="source.websiteUrl" rel="noopener noreferrer" target="_blank">
            {{ source.description }}
        </a>
    </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';

import { FeedSource } from '@/types/FeedSource.type';

export default defineComponent({
    name: 'FeedSourceComponent',

    props: {
        source: {
            type: Object as PropType<FeedSource>,
            required: true,
        },
    },

    setup() {
        return {}
    },
});
</script>

<style lang="scss">

</style>
