<script setup>
import { onMounted } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";

const router = useRouter();

onMounted(async () => {
  try {
    await axios.get("/api/logout");
  } finally {
    router.push("/");
    window.location.reload();
  }
});
</script>

<template></template>
