<template>
  <div id="ApiService">
    <button class="accordion" @click="toggleAccordion">
      <div class="service-color" v-bind:style="{ background: color }" />
      <div class="service-path">{{ service.path }}</div>
      <div class="service-name">{{ service.name }}</div>
      <div class="service-description">{{ service.description }}</div>
      <BBadge
        class="service-online"
        v-if="this.connectionStatus === 'connected'"
      >
        {{ this.connectionStatus }}
      </BBadge>
      <BFormCheckbox
        class="service-connection"
        switch
        v-model="connectService"
        size="lg"
      />
      <div v-if="expanded" class="service-arrow">
        <img class="service-arrow-icon" src="../assets/down-arrow.svg" />
      </div>
      <div v-else class="service-arrow">
        <img class="service-arrow-icon" src="../assets/right-arrow.svg" />
      </div>
    </button>
    <div class="panel" v-bind:style="{ display: panelDisplay }">
      <div v-if="service.methods.length > 0" class="service-group">
        <div class="service-group-title">
          Methods
        </div>
        <div v-for="method in service.methods" v-bind:key="method.name">
          <ApiMethod
            class="service-group-element"
            v-bind:websocket="websocket"
            v-bind:method="method"
          />
        </div>
      </div>

      <div v-if="service.notifications.length > 0" class="service-group">
        <div class="service-group-title">
          <BFormCheckbox switch v-model="enableAllNotifications" size="lg">
            Notifications
          </BFormCheckbox>
        </div>
        <div
          v-for="(notification, index) in service.notifications"
          v-bind:key="notification.name"
        >
          <ApiNotification
            class="service-group-element"
            v-bind:websocket="websocket"
            v-bind:notification="notification"
            v-bind:color="color"
            v-bind:serviceName="service.name"
            v-model="notificationsState[index]"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiMethod from "./ApiMethod.vue";
import ApiNotification from "./ApiNotification.vue";
import { BBadge, BFormCheckbox } from "bootstrap-vue";
import {
  JsonRpcWebsocket,
  WebsocketReadyStates
} from "jsonrpc-client-websocket";

const ConnectionStatus = {
  Connected: "connected",
  Disconnected: "disconnected"
};

export default {
  name: "ApiService",
  components: {
    ApiMethod,
    ApiNotification,
    BBadge,
    BFormCheckbox
  },
  data: function() {
    return {
      expanded: true,
      panelDisplay: "block",
      connectService: false,
      connectionStatus: ConnectionStatus.Disconnected,
      connectionError: "",
      websocket: void 0,
      notificationsState: [],
      enableAllNotifications: true
    };
  },
  watch: {
    enableAllNotifications: function() {
      if (this.allNotificationsEnabled != this.enableAllNotifications) {
        this.notificationsState = Array(this.service.notifications.length).fill(
          this.enableAllNotifications
        );
      }
    },
    allNotificationsEnabled: function() {
      this.enableAllNotifications = this.allNotificationsEnabled;
    },
    connectService: function() {
      if (
        (this.connectService === true &&
          (!this.websocket ||
            this.websocket.state !== WebsocketReadyStates.OPEN)) ||
        (this.connectService === false &&
          this.websocket &&
          this.websocket.state === WebsocketReadyStates.OPEN)
      ) {
        this.toggleWebsocketConnection();
      }
    }
  },
  props: {
    color: String,
    serverInfo: void 0,
    service: {
      name: String,
      path: String,
      description: String,
      methods: [],
      notifications: []
    }
  },
  created() {
    this.notificationsState = Array(this.service.notifications.length).fill(
      this.enableAllNotifications
    );
  },
  methods: {
    toggleAccordion() {
      this.expanded = !this.expanded;
      this.panelDisplay = this.expanded ? "block" : "none";
    },
    async toggleWebsocketConnection() {
      if (this.connectionStatus === ConnectionStatus.Disconnected) {
        await this.connect();
      } else {
        this.disconnect();
      }
    },
    async connect() {
      this.websocket = new JsonRpcWebsocket(
        this.wsPath,
        2000,
        this.websocketErrorCallback
      );

      try {
        await this.websocket.open();
      } catch (error) {
        this.callStatusText = "Failed to establish connection";
      }

      this.connectionStatus =
        this.websocket.state === WebsocketReadyStates.OPEN
          ? ConnectionStatus.Connected
          : ConnectionStatus.Disconnected;
      this.connectService =
        this.websocket.state === WebsocketReadyStates.OPEN ? true : false;
    },
    disconnect() {
      this.websocket.close();
      this.websocket = void 0;
      this.connectionStatus = ConnectionStatus.Disconnected;
      this.connectService = false;
      this.connectionError = "";
    },
    websocketErrorCallback(error) {
      this.connectionError = error.message;

      if (error.code !== 1006) {
        // close event
        // eslint-disable-next-line
        console.warn(error);
      }

      this.connectionStatus =
        this.websocket && this.websocket.state === WebsocketReadyStates.OPEN
          ? ConnectionStatus.Connected
          : ConnectionStatus.Disconnected;
      this.connectService =
        this.websocket && this.websocket.state === WebsocketReadyStates.OPEN
          ? true
          : false;
    }
  },
  computed: {
    wsPath: function() {
      return this.serverInfo.ws + this.service.path;
    },
    allNotificationsEnabled: function() {
      return this.notificationsState.every(x => x === true);
    }
  }
};
</script>

<style scoped lang="scss">
#ApiService {
  color: map-get($primary-color, 400);
  min-width: 600px;
  margin-bottom: 20px;

  .accordion {
    padding: 0px;
    font-family: inherit;
    background-color: map-get($primary-color, 30);
    color: inherit;
    cursor: pointer;
    height: 60px;
    width: 100%;
    border: none;
    text-align: left;
    vertical-align: middle;
    outline: none;
    font-size: 15px;
    transition: 0.4s;
    display: flex;
    border-bottom: 1px solid map-get($primary-color, 100);
    border-radius: 7px 0px 0px 7px;
  }

  .panel {
    font-family: inherit;
    padding-left: 10px;
    overflow: hidden;
  }

  .service-color {
    height: 59px;
    width: 10px;
    border-radius: 7px 0px 0px 7px;
  }

  .service-connection {
    padding: 15px 5px 0px 0px;
    margin-right: 0px;
    margin-left: auto;
  }

  .service-online {
    text-align: center;
    margin: 20px 0px 0px 20px;
    background-color: map-get($accent-color, 500);
  }

  .service-name {
    height: 40px;
    line-height: 60px;
    text-align: center;
    color: inherit;
    padding: 0px 20px 0px 20px;
    font-size: 25px;
  }

  .service-description {
    height: 40px;
    line-height: 60px;
    text-align: center;
    color: inherit;
    font-size: 15px;
  }

  .service-path {
    color: map-get($primary-color, 30);
    background-color: map-get($primary-color, 400);
    font-size: 15px;
    margin-top: 10px;
    margin-left: 10px;
    padding: 7px;
    border-radius: 5px;
    border-style: solid;
    border-color: map-get($primary-color, 400);
    border-width: 1px;
  }

  .service-arrow {
    height: 40px;
    line-height: 60px;
    margin-right: 10px;
    margin-left: 10px;
  }

  .service-arrow-icon {
    height: 40px;
    line-height: 60px;
    height: 15px;
  }

  .service-group {
    padding: 10px 10px 0px 10px;
  }

  .service-group-title {
    font-size: 20px;
  }

  .service-group-element {
    padding: 8px;
  }
}
</style>
