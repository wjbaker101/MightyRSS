<template>
    <div class="configuration-view" v-if="collections !== null">
        <HeaderComponent />
        <div class="content-width">
            <div v-if="user !== null" class="user-details content-container flex align-items-center">
                <div class="flex-2">
                    <h2>Currently logged in as:</h2>
                    <p>{{ user.username }}</p>
                </div>
                <div class="text-centered">
                    <button>Log Out</button>
                </div>
            </div>
            <div class="flex gap align-items-center">
                <h1 class="flex-auto">Collections</h1>
                <div>
                    <button @click="onCollection()">
                        <IconComponent icon="plus" gap="right" />
                        <span>New Collection</span>
                    </button>
                </div>
                <div class="flex-auto">
                    <strong class="feed-count">{{ collections.feedSourceCount }}</strong> feeds
                </div>
            </div>
            <div class="collections-details">
                <div v-for="collection in collections.collections">
                    <div class="collection-title flex gap align-items-center">
                        <h3>{{ collection.collection?.name ?? 'Not in a Collection' }}:</h3>
                        <div v-if="collection.collection !== null" class="update-collection flex-auto" @click="onCollection(collection.collection ?? undefined)">
                            <IconComponent icon="menu" />
                        </div>
                    </div>
                    <div class="feed-source flex align-items-center" v-for="feedSource in collection.feedSources">
                        <strong>
                            <a :href="feedSource.websiteUrl" target="_blank" rel="nofollow noreferrer" :title="feedSource.description">
                                {{ feedSource.title }}
                            </a>
                        </strong>
                        <div class="flex-auto">
                            <div class="delete-feed-source">
                                <IconComponent icon="cross" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import HeaderComponent from '@/components/Header.component.vue';
import CollectionModalComponent from '@/view/configuration/modal/CollectionModal.component.vue';

import { apiClient } from '@/api/api-client';
import { useModal } from '@wjb/vue/use/modal.use';

import { IUser } from '@/model/User.model';
import { IGetCollectionsDto } from '@/api/dtos/GetCollections.dto';
import { ICollection } from '@/model/Collection.model';

const modal = useModal();

const user = ref<IUser | null>(null);
const collections = ref<IGetCollectionsDto | null>(null);

const onCollection = function (collection?: ICollection): void {
    modal.show({
        component: CollectionModalComponent,
        componentProps: {
            collection,
        },
    });
};

onMounted(async () => {
    const _user = await apiClient.user.getSelf();
    user.value = _user;

    const _collections = await apiClient.collections.get();
    collections.value = _collections;
});
</script>

<style lang="scss">
.configuration-view {

    .content-container {
        padding: 1rem;
        border-radius: 0.5rem;
        box-shadow: 1px 2px 10px rgba(0, 0, 0, 0.2), 2px 3px 30px rgba(0, 0, 0, 0.2), inset 0 0 3px rgba(0, 0, 0, 0.2);
    }

    .feed-count {
        font-size: 2rem;
    }

    .feed-source,
    .collection-title {
        &:hover {
            background-color: rgba(0, 0, 0, 0.05);
        }
    }

    .collection-title {
        margin: 0.5rem -1rem;
        padding: 0.5rem 1rem;
        border-radius: 0.5rem;

        h3 {
            margin: 0;
        }
    }

    .feed-source {
        padding: 0 0.5rem;
        border-radius: calc(var(--wjb-border-radius) / 2);
        border-left: 2px solid var(--wjb-primary);

        & + .feed-source {
            margin-top: 0.25rem;
        }
    }

    .delete-feed-source,
    .update-collection {
        cursor: pointer;
        opacity: 0.2;

        &:hover {
            opacity: 1;
        }
    }
}
</style>