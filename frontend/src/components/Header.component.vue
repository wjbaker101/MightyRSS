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

<script setup lang="ts">
import { UseRss } from '@/use/Rss.use';

const useRss = UseRss();

const refreshFeed = async function (): Promise<void> {
    await useRss.load();
};
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