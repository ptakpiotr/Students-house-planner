<script setup>
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import PageTitle from "../components/PageTitle.vue";
import Plan from "../components/Plan.vue";

const router = useRouter();
const monthRef = ref("");

onMounted(() => {
  const pathSplit = router.currentRoute.value.fullPath.split("/");
  const key = pathSplit[pathSplit.length - 1];

  monthRef.value = key;
});
</script>
<template>
  <PageTitle :title="`${monthRef} month plan`" />
  <Plan :editable="false" />
</template>
