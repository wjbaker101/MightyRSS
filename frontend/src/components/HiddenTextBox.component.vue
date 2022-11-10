<template>
    <input
        ref="element"
        type="text"
        class="hidden-textbox-component"
        :value="modelValue"
        @input="oninput"
        @keypress.enter="$event.target.blur()"
        @focus="onFocus"
        @blur="onBlur"
        @dblclick="onDoubleClick"
    >
</template>

<script setup lang="ts">
import { ref } from 'vue';

const props = defineProps<{
    modelValue: string;
}>();

const emit = defineEmits(['update:modelValue', 'finish']);

const element = ref<HTMLInputElement | null>(null);

const oldValue = ref<string>(props.modelValue);
const newValue = ref<string>(props.modelValue);

const oninput = function(event: InputEvent): void {
    if (event.target === null || element.value === null)
        return;

    emit('update:modelValue', element.value.value);

    newValue.value = element.value.value;
};

const onBlur = function (): void {
    if (oldValue.value === newValue.value)
        return;

    emit('finish', newValue.value);

    oldValue.value = newValue.value;
};
</script>

<style lang="scss">
.hidden-textbox-component {
    width: 100%;
    padding: 0;
    background: none;
    box-shadow: none;
    background-color: transparent;
    border: 0;
    font: inherit;
    border: 1px solid transparent;
    letter-spacing: inherit;
    color: inherit;
    border-radius: 0.3rem;
    transition: background-color 0.2s, border-color 0.2s;

    &:active,
    &:focus {
        outline: none;
        border-color: #555;
        background-color: transparentize(#eee, 0.6);
        transition: background-color 0.4s, border-color 1s;
    }
}
</style>