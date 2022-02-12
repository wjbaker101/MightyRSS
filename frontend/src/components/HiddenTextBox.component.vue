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

<script lang="ts">
import { defineComponent, ref } from 'vue';

export default defineComponent({
    name: 'HiddenTextBoxComponent',

    props: {
        modelValue: {
            type: String,
            required: true,
        },
    },

    emits: [
        'update:modelValue',
        'finish',
    ],

    setup(props, context) {
        const element = ref<HTMLInputElement | null>(null);

        const oldValue = ref<string>(props.modelValue);
        const newValue = ref<string>(props.modelValue);

        return {
            element,

            oninput(event: InputEvent) {
                if (event.target === null || element.value === null)
                    return;

                context.emit('update:modelValue', element.value.value);

                newValue.value = element.value.value;
            },

            onBlur() {
                if (oldValue.value === newValue.value)
                    return;

                context.emit('finish', newValue.value);

                oldValue.value = newValue.value;
            },
        }
    },
});
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