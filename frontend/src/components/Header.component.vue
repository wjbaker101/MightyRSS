<template>
    <div class="header-component text-centered">
        <h1>
            <span class="refresh-container" @click="refreshFeed">
                <img class="swords" width="48" height="48" src="@/assets/swords.svg">
                <span class="text">Mighty RSS</span>
            </span>
        </h1>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

import { UseRss } from '@/use/Rss.use';

export default defineComponent({
    name: 'HeaderComponent',

    setup() {
        const useRss = UseRss();

        return {
            async refreshFeed() {
                await useRss.load();
            },
        }
    },
});
</script>

<style lang="scss">
.header-component {
    .refresh-container {
        cursor: pointer;
        user-select: none;

        & > * {
            vertical-align: middle;
        }
    }

    .swords {
        margin-right: 1rem;
    }
}
</style>
