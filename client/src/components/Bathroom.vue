<script setup>
import { reactive } from "vue";
import { Lock } from "lucide-vue-next";
import axios from "axios";

const props = defineProps({
  editable: {
    type: Boolean,
    default: false,
  },
});

const state = reactive({
  weeks: [null, null, null, null],
});

const userId = document.cookie.split("=")[1];

async function clickWeek(idx) {
  if (props.editable) {
    if (state.weeks[idx] === userId) {
      state.weeks[idx] = null;
    } else if (!Object.values(state.weeks).includes(userId)) {
      state.weeks[idx] = userId;
    }

    const urlSplit = window.location.href.split("/");
    const month = urlSplit[urlSplit.length - 1];

    await axios.post(`/api/bathroom/${month}`, state, {
      headers: {
        "Content-Type": "application/json",
      },
      withCredentials: true,
    });
  }
}
</script>
<template>
  <div class="h-1/2">
    <p class="flex gap-x-2">Bathroom <Lock v-if="!props.editable" /></p>
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
