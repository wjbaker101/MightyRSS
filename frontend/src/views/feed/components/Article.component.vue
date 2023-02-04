<template>
    <a class="article-component" :href="article.url" rel="noopener noreferrer" target="_blank">
        <img width="50" class="sword-indicator" src="@/assets/sword.svg">
        <article>
            <h3>{{ article.title }}</h3>
            <p class="published-at">
                <span>{{ (article.source.titleAlias ?? article.source.title) }} &mdash; </span>
                <small>{{ displayPublishedAt }}</small>
            </p>
        </article>
    </a>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

import { IFeedArticle } from '@/model/FeedArticle.model';

const props = defineProps<{
    article: IFeedArticle;
}>();

const getFormattedDate = function (date: Dayjs): string {
    const now = dayjs();

    if (date.month() === now.month() && date.year() === now.year())
        return date.format('dddd Do');

    if (date.year() !== now.year())
        return date.format('dddd Do MMMM YYYY');

    return date.format('dddd Do MMMM');
};

const displayPublishedAt = computed<string>(() => {
    const date = props.article.publishedAt;

    const formattedDate = getFormattedDate(date);

    return `${formattedDate} (${date.fromNow()})`;
});
</script>

<style lang="scss">
.article-component {
    display: block;
    position: relative;
    color: inherit;
    text-decoration: none;

    &:visited {
        color: #666;
    }

    .sword-indicator {
        position: absolute;
        left: -1rem;
        top: 50%;
        transform: translate(calc(-100% - 1rem), -50%);
        opacity: 0;
        transition: opacity 0.3s, transform 0.2s;
    }

    &:hover .sword-indicator {
        opacity: 1;
        transform: translate(-100%, -50%);
    }

    .published-at {
        margin-top: -1rem;
    }
}
</style>