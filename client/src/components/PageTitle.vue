<script setup>
const props = defineProps({
  title: {
    type: String,
    required: true,
  },
});
</script>
<template>
  <p class="text-xl text-blue-950 uppercase m-2 p-1">{{ props.title }}</p>
</template>
