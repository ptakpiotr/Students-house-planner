<script setup>
import { reactive } from "vue";
import { Lock } from "lucide-vue-next";

const props = defineProps({
  editable: {
    type: Boolean,
    default: false,
  },
});

const state = reactive({
  weeks: [null, null, null, null],
});

const userId = "USER_ABC";

function clickWeek(idx) {
  if (props.editable) {
    if (state.weeks[idx] === userId) {
      state.weeks[idx] = null;
    } else if (!Object.values(state.weeks).includes(userId)) {
      state.weeks[idx] = userId;
    }
  }
}
</script>
<template>
  <div class="h-1/2">
    <p class="flex gap-x-2">Kitchen <Lock v-if="!props.editable" /></p>
    <ul>
      <li
        v-for="(week, index) in state.weeks"
        v-bind:key="week"
        @click="() => clickWeek(index)"
      >
        {{ `${index + 1} week: ` }}{{ week }}
      </li>
    </ul>
  </div>
</template>
