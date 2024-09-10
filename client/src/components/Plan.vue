<script setup>
import Shopping from "../components/Shopping.vue";
import Bathroom from "../components/Bathroom.vue";
import Kitchen from "../components/Kitchen.vue";

const props = defineProps({
  editable: {
    type: Boolean,
    default: false,
  },
});
</script>
<template>
  <div class="border border-slate-300 w-full h-[75vh] flex">
    <Shopping class="flex-1" :editable="props.editable" />
    <div class="flex-1 flex flex-col">
      <Bathroom :editable="props.editable" />
      <Kitchen :editable="props.editable" />
    </div>
  </div>
</template>
