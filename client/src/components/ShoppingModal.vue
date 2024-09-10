<script setup>
import { PlusCircle, PanelBottomClose } from "lucide-vue-next";
import { reactive } from "vue";
import AppButton from "./AppButton.vue";

const state = reactive({
  date: null,
  item: null,
});

const props = defineProps({
  closeModal: {
    type: Function,
    required: true,
  },
  addShoppingData: {
    type: Function,
    required: true,
  },
});

function submitForm(e) {
  e.preventDefault();

  if (props.addShoppingData) {
    props.addShoppingData({
      ...state,
      confirmed: false,
      person: "",
    });
  }

  props.closeModal();
}
</script>

<template>
  <teleport to="body">
    <div class="absolute bottom-0 w-full">
      <div class="bg-slate-300">
        <div class="flex w-full">
          <p class="text-md p-2 m-2 flex-1">Add shopping info</p>
          <AppButton class="mr-4" @click="props.closeModal">
            <template #content>
              <PanelBottomClose />
            </template>
          </AppButton>
        </div>
        <form
          ref="formRef"
          class="shopping-form flex flex-col translate-x-1/2 mr-36 gap-y-4"
          @submit.prevent="submitForm"
        >
          <input type="date" placeholder="Date" v-model="state.date" required />
          <input type="text" placeholder="Item" v-model="state.item" required />
          <AppButton class="w-40">
            <template #content>
              <span class="text-xs flex justify-center gap-x-2"
                >Add <PlusCircle size="1rem"
              /></span>
            </template>
          </AppButton>
        </form>
      </div>
    </div>
  </teleport>
</template>
