import Vue from "vue";
import JsonRpcDoc from "./JsonRpcDoc.vue";

Vue.config.productionTip = false;

new Vue({
  render: h => h(JsonRpcDoc)
}).$mount("#JsonRpcDoc");
