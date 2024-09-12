<script setup>
import { PlusCircle } from "lucide-vue-next";
import { reactive, ref } from "vue";
import AppButton from "../components/AppButton.vue";
import ShoppingModal from "../components/ShoppingModal.vue";

const showModal = ref(false);

const props = defineProps({
  editable: {
    type: Boolean,
    default: false,
  },
  shopping: {
    type: Array,
    default: false,
    required: true,
  },
  addShoppingData: {
    type: Function,
    required: true,
  },
});

function openModal() {
  showModal.value = true;
}

function closeModal() {
  showModal.value = false;
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
        :addShoppingData="props.addShoppingData"
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
        <tr v-for="item in props.shopping" v-bind:key="item?.id">
          <td>{{ item.Date }}</td>
          <td>{{ item.Item }}</td>
          <td>{{ item.Person }}</td>
          <td>
            <input type="checkbox" v-model="item.Confirmed" />
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
