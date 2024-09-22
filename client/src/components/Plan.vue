<script setup>
import Shopping from "../components/Shopping.vue";
import Bathroom from "../components/Bathroom.vue";
import Kitchen from "../components/Kitchen.vue";
import { useRouter } from "vue-router";
import { onMounted, reactive } from "vue";
import axios from "axios";

const props = defineProps({
  editable: {
    type: Boolean,
    default: false,
  },
});

const router = useRouter();

const state = reactive({
  monthData: null,
});

onMounted(async () => {
  const pathSplit = router.currentRoute.value.fullPath.split("/");
  const key = pathSplit[pathSplit.length - 1];

  const resp = await axios.get(`/api/month/${key}`, { withCredentials: true });
  const data = resp.data;
  
  state.monthData = data;
});

async function addShoppingData(data) {
  if (Array.isArray(state?.monthData?.ShoppingEntries)) {
    const item = {
      Item: data.item,
      MonthId: state.monthData.Id,
      Confirmed: false,
      Date: new Date(data.date),
    };

    await axios.post(`/api/shopping`, item, {
      headers: {
        "Content-Type": "application/json",
      },
      withCredentials: true,
    });

    state.monthData.ShoppingEntries.push({ ...item, Person: "" });
  }
}
</script>
<template>
  <div class="border border-slate-300 w-full h-[75vh] flex">
    <Shopping
      class="flex-1"
      :editable="props.editable"
      :addShoppingData="addShoppingData"
      :shopping="state.monthData?.ShoppingEntries"
    />
    <div class="flex-1 flex flex-col">
      <Bathroom :editable="props.editable" />
      <Kitchen :editable="props.editable" />
    </div>
  </div>
</template>
