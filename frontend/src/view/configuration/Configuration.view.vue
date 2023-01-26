<template>
    <div class="configuration-view">
        <HeaderComponent />
        <div class="content-width">
            <div class="user-details content-container">
                <h2>Currently logged in as:</h2>
                <p>TestUsername</p>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import HeaderComponent from '@/components/Header.component.vue';
</script>

<style lang="scss">
.configuration-view {

    .content-container {
        padding: 2rem 1rem;
        border-radius: 0.5rem;
        box-shadow: 1px 2px 10px rgba(0, 0, 0, 0.2), 2px 3px 30px rgba(0, 0, 0, 0.2), inset 0 0 3px rgba(0, 0, 0, 0.2);

        & > :first-child {
            margin-top: 0;
        }

        & > :last-child {
            margin-bottom: 0;
        }
    }
}
</style>