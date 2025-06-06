<template>
    <div class="configuration-view" v-if="collections !== null">
        <div class="content-width">
            <CardComponent v-if="user !== null" class="user-details flex align-items-center gap">
                <div class="flex-auto">
                    <div class="return-button" title="Return to feed" @click="$router.push({ path: '/' })">
                        <IconComponent icon="arrow-left" />
                    </div>
                </div>
                <div class="flex-2">
                    <h2>Currently logged in as:</h2>
                    <p>{{ user.username }}</p>
                </div>
                <div class="text-centered">
                    <button @click="onLogOut">Log Out</button>
                </div>
            </CardComponent>
            <div class="collections-title flex gap">
                <h1 class="flex-auto">Collections</h1>
                <div class="flex-auto">
                    (<strong class="feed-count">{{ collections.feedSourceCount }}</strong> feeds)
                </div>
                <div></div>
                <div class="flex-auto">
                    <button @click="onNewFeed()">
                        <IconComponent icon="plus" gap="right" />
                        <span>Feed</span>
                    </button>
                </div>
                <div class="flex-auto">
                    <button @click="onCollection()">
                        <IconComponent icon="plus" gap="right" />
                        <span>Collection</span>
                    </button>
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
                            <div class="delete-feed-source" @click="onFeedSourceClick(feedSource)">
                                <IconComponent icon="menu" />
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

import CollectionModalComponent from '@/views/configuration/modals/CollectionModal.component.vue';
import FeedModalComponent from '@/views/configuration/modals/FeedModal.component.vue';

import { apiClient } from '@/api/api-client';
import { useEvents } from '@/use/events/events.use';
import { useModal } from '@wjb/vue/use/modal.use';

import { IUser } from '@/model/User.model';
import { ICollection } from '@/model/Collection.model';
import { IFeedSource } from '@/model/FeedSource.model';
import { IGetCollectionsDto } from '@/api/dtos/GetCollections.dto';

const events = useEvents();
const modal = useModal();

const user = ref<IUser | null>(null);
const collections = ref<IGetCollectionsDto | null>(null);

const onNewFeed = function (): void {
    modal.show({
        component: FeedModalComponent,
        componentProps: {},
    });
};

const onCollection = function (collection?: ICollection): void {
    modal.show({
        component: CollectionModalComponent,
        componentProps: {
            collection,
        },
    });
};

const onLogOut = function (): void {
    events.publish('TRIGGER_LOG_OUT', {});
};

const onFeedSourceClick = function (feedSource: IFeedSource): void {
    modal.show({
        component: FeedModalComponent,
        componentProps: {
            feedSource,
        },
    });
};

onMounted(async () => {
    const _user = await apiClient.user.getSelf();
    if (!(_user instanceof Error))
        user.value = _user;

    const _collections = await apiClient.collections.get();
    if (!(_collections instanceof Error))
        collections.value = _collections;
});
</script>

<style lang="scss">
.configuration-view {

    .return-button {
        padding: 1rem;
        border-radius: 0.5rem;
        cursor: pointer;

        &:hover {
            background-color: rgba(0, 0, 0, 0.05);
        }
    }

    .collections-title {
        padding-top: 1rem;
        align-items: flex-end;

        h1 {
            margin: 0;
            line-height: 1em;
        }
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