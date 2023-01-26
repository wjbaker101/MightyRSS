<template>
    <div class="configuration-view">
        <HeaderComponent />
        <div class="content-width">
            <div v-if="user !== null" class="user-details content-container flex align-items-center">
                <div class="flex-2">
                    <h2>Currently logged in as:</h2>
                    <p>{{ user.username }}</p>
                </div>
                <div class="text-centered">
                    <strong class="feed-count">5</strong> feeds
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import HeaderComponent from '@/components/Header.component.vue';

import { userApi } from '@/api/user/user.api';
import { IUser } from '@/model/User.model';

const user = ref<IUser | null>(null);

onMounted(async () => {
    const result = await userApi.getSelf();

    user.value = result;
});
</script>

<style lang="scss">
.configuration-view {

    .content-container {
        padding: 1rem;
        border-radius: 0.5rem;
        box-shadow: 1px 2px 10px rgba(0, 0, 0, 0.2), 2px 3px 30px rgba(0, 0, 0, 0.2), inset 0 0 3px rgba(0, 0, 0, 0.2);
    }

    .user-details {
        .feed-count {
            font-size: 2rem;
        }
    }
}
</style>