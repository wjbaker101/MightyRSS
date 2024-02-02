<template>
    <header class="header-component text-centered">
        <h1>
            <img width="48" height="48" src="@/assets/swords.svg">
            <span>Mighty RSS</span>
        </h1>
    </header>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.header-component {
    h1 {
        padding: 2rem 0 1rem 0;
    }

    img,
    span {
        vertical-align: middle;
    }

    img {
        margin-right: 1rem;
    }
}
</style>