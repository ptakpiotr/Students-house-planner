<script setup>
import { PlusCircle } from "lucide-vue-next";
import { reactive, ref } from "vue";
import AppButton from "../components/AppButton.vue";
import ShoppingModal from "../components/ShoppingModal.vue";

const showModal = ref(false);
const state = reactive({
  shopping: [],
});

const props = defineProps({
  editable: {
    type: Boolean,
    default: false,
  },
});

function openModal() {
  showModal.value = true;
}

function closeModal() {
  showModal.value = false;
}

function addShoppingData(data) {
  console.log(data);
  state.shopping.push(data);
}
</script>
<template>
  <div>
    <div class="flex items-center">
      <h6 class="text-blue-900 flex-1">Shopping</h6>
      <AppButton v-if="props.editable" class="mr-10" @click="openModal">
        <template #content>
          <span class="text-xs flex gap-x-2"
            >Add <PlusCircle size="1rem"
          /></span>
        </template>
      </AppButton>
      <ShoppingModal
        v-if="props.editable && showModal"
        :closeModal="closeModal"
        :addShoppingData="addShoppingData"
      />
    </div>
    <table class="shopping-table">
      <thead>
        <tr>
          <th>Date</th>
          <th>Item</th>
          <th>Person</th>
          <th>Confirmed</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in state.shopping" v-bind:key="item?.id">
          <td>{{ item.date }}</td>
          <td>{{ item.item }}</td>
          <td>{{ item.person }}</td>
          <td>
            <input type="checkbox" v-model="item.confirmed" />
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
