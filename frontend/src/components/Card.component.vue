<template>
    <div class="card-component">
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.card-component {
    padding: 1rem;
    border-radius: var(--wjb-border-radius);
    box-shadow: 1px 2px 10px rgba(0, 0, 0, 0.2), 2px 3px 30px rgba(0, 0, 0, 0.2), inset 0 0 3px rgba(0, 0, 0, 0.2);
}
</style>