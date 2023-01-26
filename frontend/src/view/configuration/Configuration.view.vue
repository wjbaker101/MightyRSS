<template>
    <div class="configuration-view">
        <HeaderComponent />
        <div class="content-width">
            <div v-if="user !== null" class="user-details content-container flex align-items-center">
                <div class="flex-2">
                    <h2>Currently logged in as:</h2>
                    <p>{{ user.username }}</p>
                </div>
                <div v-if="configuration !== null" class="text-centered">
                    <strong class="feed-count">{{ configuration.collections.flatMap(x => x.feedSources).length }}</strong> feeds
                </div>
            </div>
            <div v-if="configuration !== null" class="collections-details">
                <div v-for="collection in configuration.collections">
                    <h3>{{ collection.collection ?? 'Not in a Collection' }}</h3>
                    <div class="feed-source" v-for="feedSource in collection.feedSources">
                        <strong>
                            <a :href="feedSource.feedSource.websiteUrl" target="_blank" rel="nofollow noreferrer">
                                {{ feedSource.userFeedSource.titleAlias ?? feedSource.feedSource.title }}
                            </a>
                        </strong>
                    </div>
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
import { IGetConfigurationDto } from '@/api/configuration/dtos/GetConfiguration.dto';
import { configurationApi } from '@/api/configuration/configuration.api';

const user = ref<IUser | null>(null);
const configuration = ref<IGetConfigurationDto | null>(null);

onMounted(async () => {
    const result = await userApi.getSelf();
    user.value = result;

    const configurationResult = await configurationApi.getConfiguration();
    configuration.value = configurationResult;
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

    .feed-source {
        & + .feed-source {
            margin-top: 0.25rem;
        }
    }
}
</style>