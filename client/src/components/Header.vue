<script setup>
import HeaderLink from "./HeaderLink.vue";
</script>
<template>
  <nav class="bg-blue-600 p-4 flex">
    <p class="text-xs py-2 text-blue-200 font-bold uppercase flex-1">
        Student's house planner
    </p>
    <ul class="flex gap-x-6 text-white">
      <HeaderLink link="/" text="Home" />
      <HeaderLink link="/about" text="About" />
      <HeaderLink link="/logout" text="Logout" />
    </ul>
  </nav>
</template>
