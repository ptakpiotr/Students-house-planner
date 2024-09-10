<template>
  <li>
    <router-link class="font-semibold text-xl p-1 m-1 hover:text-blue-400" :to="link">{{ text }}</router-link>
  </li>
</template>
<script>
export default {
  props: {
    link: {
      type: String,
      required: true,
    },
    text: {
      type: String,
      required: true,
    },
  },
  setup(props) {
    return {
      ...props,
    };
  },
};
</script>
