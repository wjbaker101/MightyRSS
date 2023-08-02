<template>
    <HeaderComponent />
    <RouterView></RouterView>
    <ModalComponent />
</template>

<script setup lang="ts">
import HeaderComponent from '@/components/Header.component.vue';
</script>

<style lang="scss">
@use '@/style/webstyle' as *;
</style>