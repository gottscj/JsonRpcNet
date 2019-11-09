import Vue from "vue";
import JsonRpcDoc from "./JsonRpcDoc.vue";
import { TypeDefinitionsService } from "./services/TypeDefinitions.service";

Vue.config.productionTip = false;

new Vue({
  render: h => h(JsonRpcDoc),
  data: {
    typeDefinitionsService: TypeDefinitionsService
  }
}).$mount("#JsonRpcDoc");
