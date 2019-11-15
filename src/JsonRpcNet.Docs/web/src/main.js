import Vue from "vue";
import JsonRpcDoc from "./JsonRpcDoc.vue";
import { TypeDefinitionsService } from "./services/TypeDefinitions.service";
import { NotificationsService } from "./services/Notifications.service";

Vue.config.productionTip = false;

new Vue({
  render: h => h(JsonRpcDoc),
  data: {
    typeDefinitionsService: TypeDefinitionsService,
    notificationsService: new NotificationsService()
  }
}).$mount("#JsonRpcDoc");
