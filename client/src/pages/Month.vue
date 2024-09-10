<script setup>
import PageTitle from "../components/PageTitle.vue";
import Plan from "../components/Plan.vue";
</script>
<template>
  <PageTitle title="Current month plan" />
  <Plan :editable="true" />
</template>
