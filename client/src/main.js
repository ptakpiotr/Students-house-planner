import { createApp } from 'vue'
import { router } from './router';
import vSelect from 'vue-select'
import App from './App.vue'
import 'vue-select/dist/vue-select.css';
import './style.css'

const app = createApp(App);
app.use(router);
app.component('v-select', vSelect)
app.mount('#app');
