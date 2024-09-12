<script setup>
import AppButton from "../components/AppButton.vue";
import PageTitle from "../components/PageTitle.vue";
import { onMounted, reactive, ref } from "vue";
import { Navigation } from "lucide-vue-next";
import axios from "axios";
import { router } from "../router";

const state = reactive({
  months: [],
});
const selectedValue = ref(null);

onMounted(async () => {
  const resp = await axios.get("/api/months", { withCredentials: true });
  const data = resp.data;
  state.months = data;
});

function goToMonth() {
  const page = selectedValue.value?.code;

  const date = new Date();
  const currentKey = `${date.getFullYear()}_${date.getMonth() + 1}`;

  if (page === currentKey) {
    router.push(`/month/${page}`);
  } else if (page) {
    router.push(`/history/${page}`);
  }
}
</script>
<template>
  <PageTitle title="Home" />
  <div class="w-full flex flex-col items-center">
    <p class="mb-1">Choose month:</p>
    <v-select
      v-if="state?.months"
      class="min-w-48 mb-4"
      :options="state.months"
      v-model="selectedValue"
    />
    <AppButton @click="goToMonth">
      <template #content>
        <span class="flex gap-x-2">Navigate <Navigation /></span>
      </template>
    </AppButton>
  </div>
</template>
